using Kafka.Messaging.Services.Abstractions;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using ProjectService.Models;
using SharedLibrary.Auth;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Models.AnalyticModels;
using SharedLibrary.Models.KafkaModel;
using SharedLibrary.Models;
using SharedLibrary.Entities;

namespace ProjectService.BusinessLayer.Implementations;

public class ItemManager(
    IItemRepository itemRepository,
    IValidateItemManager validatorManager,
    IItemBoardsRepository itemBoardsRepository,
    IProjectRepository projectRepository,
    ICommentRepository commentRepository,
    IAttachmentRepository attachmentRepository,
    IUserRepository userRepository,
    IKafkaProducer<TaskEventMessage> kafkaProducer,
    HttpClient httpClient,
    IAuth auth) : IItemManager
{
    public async Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token)
    {
        await validatorManager.ValidateCreateAsync(createItemModel);

        var currentUserId = auth.GetCurrentUserId();

        var project = await projectRepository.GetByBoardIdAsync(createItemModel.BoardId);

        var item = createItemModel.Item;
        var entity = ItemMapper.ItemToEntity(item);
        entity.CreatedAt = DateTime.UtcNow;

        await itemRepository.CreateAsync(entity);

        entity.BusinessId = $"{project.Key}-ITEM-{entity.Id}";
        entity.AuthorId = currentUserId;

        await itemBoardsRepository.Create(
            new ItemBoardEntity
            {
                ItemId = entity.Id,
                BoardId = createItemModel.BoardId,
                StatusId = (int)entity.StatusId!
            }
        );
        
        entity = await itemRepository.GetByIdAsync(entity.Id);

        var model = new SharedLibrary.Models.AnalyticModels.TaskHistoryModel
        {
            FieldName = "Новое задание",
            OldValue = "",
            NewValue = item.Title,
            ItemId = item.Id,
            UserId = (int)auth.GetCurrentUserId()!,
            ChangedAt = DateTime.UtcNow
        };

        await httpClient.PostAsJsonAsync("create", model);

        return entity.Id;
    }

    public async Task Delete(int id)
    {
        await itemRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ItemModel>> GetAllItemsAsync()
    {
        var items = await itemRepository.GetItemsAsync();

        var models = await Task.WhenAll(items.Select(x=>ItemMapper.ToModel(x, userRepository)));

        return models;
    }


    public async Task<ItemModel> GetByIdAsync(int? id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));
        var model = await ItemMapper.ToModel(await itemRepository.GetByIdAsync((int)id), userRepository);
        return model;
    }

    public async Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId)
    {
        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);
        var models = await Task.WhenAll(items.Select(x=>ItemMapper.ToModel(x, userRepository)));
        return models;
    }

    public async Task<int> UpdateAsync(ItemModel item, CancellationToken token, string message, string oldValue, string newValue, string fieldName,
        TaskEventType eventType = TaskEventType.Updated)
    {
        await validatorManager.ValidateItemModelAsync(item);
        var entity = ItemMapper.ItemToEntity(item);
        entity.Id = item.Id;
        var updatedAt = DateTime.UtcNow;
        entity.UpdatedAt = updatedAt;
        await itemRepository.UpdateAsync(entity);
        await kafkaProducer.ProduceAsync(new TaskEventMessage
        {
            EventType = eventType,
            UserItems = item.UserItems,
            Message = message,
        }, token);
        var model = new SharedLibrary.Models.AnalyticModels.TaskHistoryModel
        {
            FieldName = fieldName,
            OldValue = oldValue,
            NewValue = newValue,
            ItemId = item.Id,
            UserId = (int)auth.GetCurrentUserId()!,
            ChangedAt = updatedAt
        };
        await httpClient.PostAsJsonAsync("create", model, cancellationToken: token);
        return entity.Id;
    }

    public async Task<int> AddUserToItemAsync(int newUserId, int itemId, CancellationToken cancellationToken)
    {
        var item = await itemRepository.GetByIdAsync(itemId);
        await validatorManager.ValidateAddUserToItemAsync((int)item.ProjectId!, newUserId, itemId);
        var itemUserEntity = new UserItemEntity
        {
            ItemId = itemId,
            UserId = newUserId
        };

        var oldValue = new List<UserItemEntity>(item.UserItems);
        await itemRepository.AddUserToItemAsync(itemUserEntity);
        item.UserItems.Add(itemUserEntity);
        await UpdateAsync(await ItemMapper.ToModel(item, userRepository), cancellationToken, 
            $"В {item.Title} добавлен пользователь с айди {newUserId}", oldValue.ToString(), item.UserItems.ToString(), 
            "UserItems", TaskEventType.AddedUser); //пока оставил список юзеров через тустринг, потому решу как правильно передавать старое и новое значение
        //TODO ПРИДУМАТЬ!!!
        return itemUserEntity.Id;
    }

    public async Task<ICollection<ItemModel>> GetArchievedItemsInProject(int projectId)
    {
        await validatorManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByProjectIdAsync(projectId);
        var models = await Task.WhenAll(items.Where(x=>x.IsArchived).Select(x=>ItemMapper.ToModel(x, userRepository)));

        return models;
    }

    public async Task<ICollection<ItemModel>> GetArchievedItemsInBoard(int boardId)
    {
        var projectId = (await projectRepository.GetByBoardIdAsync(boardId)).Id;
        await validatorManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);
        var models = await Task.WhenAll(items.Where(x => x.IsArchived).Select(x=> ItemMapper.ToModel(x, userRepository)));

        return models;
    }

    public async Task<ICollection<ItemModel>> GetBugsItemsInProject(int projectId)
    {
        await validatorManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByProjectIdAsync(projectId);
        var models = await Task.WhenAll(items.Where(x => x.ItemTypeId == ItemType.BUG).Select(x => ItemMapper.ToModel(x, userRepository)));

        return models;
    }

    public async Task<ICollection<ItemModel>> GetBugsItemsInBoard(int boardId)
    {
        var projectId = (await projectRepository.GetByBoardIdAsync(boardId)).Id;
        await validatorManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);
        var models = await Task.WhenAll(items.Where(x => x.ItemTypeId == ItemType.BUG).Select(x => ItemMapper.ToModel(x, userRepository)));

        return models;
    }

    public async Task<ICollection<ItemModel>> GetByProjectIdAsync(int projectId)
    {
        await validatorManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByProjectIdAsync(projectId);
        var models = await Task.WhenAll(items.Select(x=> ItemMapper.ToModel(x, userRepository)));
        return models;
    }

    public async Task<ItemModel> GetByTitle(string title)
    {
        return await ItemMapper.ToModel(await itemRepository.GetByNameAsync(title), userRepository);
    }


    public async Task<ICollection<ItemModel>> GetCurrentUserItems()
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();
        var items = await itemRepository.GetCurrentUserItemsAsync((int)userId);
        var models = await Task.WhenAll(items.Select(x=> ItemMapper.ToModel(x, userRepository)));

        return models;
    }

    public async Task<ICollection<ItemModel>> GetItemsByUserId(int userId, int projectId)
    {
        await validatorManager.ValidateAddUserToItemAsync(projectId, userId);
        var items = await itemRepository.GetItemsByUserIdAsync(userId, projectId);
        var models = await Task.WhenAll(items.Select(x=> ItemMapper.ToModel(x, userRepository)));

        return models;
    }

    public async Task<int> AddCommentToItemAsync(CommentModel commentModel, IFormFile? attachment)
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();
        
        var item = await itemRepository.GetByIdAsync(commentModel.ItemId);
        if (item is null) throw new ItemNotFoundException();

        await validatorManager.ValidateUserInProjectAsync(item.ProjectId);
        var commentEntity = CommentMapper.ToEntity(commentModel);

        commentEntity.AuthorId = (int)userId;

        await commentRepository.CreateAsync(commentEntity);

        if (attachment is not null)
        {
            var docPath = Environment.GetEnvironmentVariable("ATTACHMENT_STORAGE_PATH");

            if (string.IsNullOrEmpty(docPath))
                throw new ArgumentNullException("Переменная окружения ATTACHMENT_STORAGE_PATH не задана");

            Directory.CreateDirectory(docPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{attachment.FileName}";

            var filePath = Path.Combine(docPath, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            docPath = $"/attachments/{uniqueFileName}";

            var attachmentEntity = new AttachmentEntity 
            { 
                AuthorId = (int)userId ,
                UploadedAt = DateTime.UtcNow,
                CommentId = commentEntity.Id,
                FilePath = docPath,
            };

            await attachmentRepository.CreateAsync(attachmentEntity);

           
        }

        var model = new SharedLibrary.Models.AnalyticModels.TaskHistoryModel
        {
            FieldName = "Комментарий",
            OldValue = "",
            NewValue = commentModel.Text,
            ItemId = item.Id,
            UserId = (int)auth.GetCurrentUserId()!,
            ChangedAt = DateTime.UtcNow
        };

        await httpClient.PostAsJsonAsync("create", model);

        return commentEntity.Id;
    }

    public async Task<ICollection<CommentModel>> GetComments(int itemId)
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();

        var item = await itemRepository.GetByIdAsync(itemId);
        if (item is null) throw new ItemNotFoundException();

        await validatorManager.ValidateUserInProjectAsync(item.ProjectId);

        var comments = commentRepository.GetByItemId(itemId);

        var commentsModels = await Task.WhenAll(
            comments.Select(c => CommentMapper.ToModel(c, userRepository))
        );

        return commentsModels.ToList();
    }
}