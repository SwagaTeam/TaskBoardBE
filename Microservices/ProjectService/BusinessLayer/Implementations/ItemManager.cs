using Kafka.Messaging.Services.Abstractions;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.DataLayer.Repositories.Implementations;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using ProjectService.Models;
using SharedLibrary.Auth;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Implementations;

public class ItemManager(
    IItemRepository itemRepository,
    IValidateItemManager validateItemManager,
    IKafkaProducer<ItemModel> kafkaProducer,
    IItemBoardsRepository itemBoardsRepository,
    IStatusRepository statusRepository,
    IProjectRepository projectRepository,
    IUserProjectManager userProjectManager,
    ICommentRepository commentRepository,
    IAttachmentRepository attachmentRepository,
    IAuth auth) : IItemManager
{
    public async Task<int> CreateAsync(CreateItemModel createItemModel, CancellationToken token)
    {
        await validateItemManager.ValidateCreateAsync(createItemModel);

        var project = await projectRepository.GetByBoardIdAsync(createItemModel.BoardId);

        var item = createItemModel.Item;
        var entity = ItemMapper.ItemToEntity(item);
        entity.CreatedAt = DateTime.UtcNow;

        await itemRepository.CreateAsync(entity);

        entity.BusinessId = $"{project.Key}-ITEM-{entity.Id}";

        await itemBoardsRepository.Create(
            new ItemBoardEntity
            {
                ItemId = entity.Id,
                BoardId = createItemModel.BoardId
            }
        );

        return entity.Id;
    }

    public async Task Delete(int id)
    {
        await itemRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ItemModel>> GetAllItemsAsync()
    {
        var items = (await itemRepository.GetItemsAsync())
            .Select(ItemMapper.ToModel);
        return items;
    }


    public async Task<ItemModel> GetByIdAsync(int? id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));
        var model = ItemMapper.ToModel(await itemRepository.GetByIdAsync((int)id));
        return model;
    }

    public async Task<ICollection<ItemModel>> GetByBoardIdAsync(int boardId)
    {
        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);
        return items.Select(ItemMapper.ToModel).ToList();
    }

    public async Task<int> UpdateAsync(ItemModel item)
    {
        await validateItemManager.ValidateItemModelAsync(item);
        var entity = ItemMapper.ItemToEntity(item);
        entity.Id = item.Id;
        entity.UpdatedAt = DateTime.UtcNow;
        await itemRepository.UpdateAsync(entity);
        return entity.Id;
    }

    public async Task<int> AddUserToItemAsync(int newUserId, int itemId)
    {
        var item = await GetByIdAsync(itemId);
        await UpdateAsync(item); // назначили человека на задачу, обновляем UpdatedAt
        await validateItemManager.ValidateAddUserToItemAsync((int)item.ProjectId, newUserId);
        var itemUserEntity = new UserItemEntity
        {
            ItemId = itemId,
            UserId = newUserId
        };
        await itemRepository.AddUserToItemAsync(itemUserEntity);
        return itemUserEntity.Id;
    }

    public async Task<ICollection<ItemModel>> GetArchievedItemsInProject(int projectId)
    {
        await validateItemManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByProjectIdAsync(projectId);
        return items.Where(x => x.IsArchived).Select(ItemMapper.ToModel).ToList();
    }

    public async Task<ICollection<ItemModel>> GetArchievedItemsInBoard(int boardId)
    {
        var projectId = (await projectRepository.GetByBoardIdAsync(boardId)).Id;
        await validateItemManager.ValidateUserInProjectAsync(projectId);
        var items = await itemRepository.GetItemsByBoardIdAsync(boardId);
        return items.Where(x => x.IsArchived).Select(ItemMapper.ToModel).ToList();
    }

    public async Task<ItemModel> GetByTitle(string title)
    {
        return ItemMapper.ToModel(await itemRepository.GetByNameAsync(title));
    }


    public async Task<ICollection<ItemModel>> GetCurrentUserItems()
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();
        var items = await itemRepository.GetCurrentUserItemsAsync((int)userId);
        return items.Select(ItemMapper.ToModel).ToList();
    }

    public async Task<ICollection<ItemModel>> GetItemsByUserId(int userId, int projectId)
    {
        await validateItemManager.ValidateAddUserToItemAsync(projectId, userId);
        var items = await itemRepository.GetItemsByUserIdAsync(userId, projectId);
        return items.Select(ItemMapper.ToModel).ToList();
    }

    public async Task<int> AddCommentToItemAsync(CommentModel commentModel, IFormFile? attachment)
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();

        var item = await itemRepository.GetByIdAsync(commentModel.ItemId);
        if (item is null) throw new ItemNotFoundException();

        if(!await userProjectManager.IsUserInProjectAsync((int)userId, (int)item.ProjectId!))
            throw new NotAuthorizedException("Пользователь не состоит в проекте");

        var commentEntity = CommentMapper.ToEntity(commentModel);

        commentEntity.AuthorId = (int)userId;

        await commentRepository.CreateAsync(commentEntity);

        if (attachment is not null)
        {
            var docPath = Environment.GetEnvironmentVariable("ATTACHMENT_STORAGE_PATH");

            if (string.IsNullOrEmpty(docPath))
                throw new Exception("Переменная окружения ATTACHMENT_STORAGE_PATH не задана");

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

        return commentEntity.Id;
    }

    public async Task<ICollection<CommentModel>> GetComments(int itemId)
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();

        var item = await itemRepository.GetByIdAsync(itemId);
        if (item is null) throw new ItemNotFoundException();

        if (!await userProjectManager.IsUserInProjectAsync((int)userId, (int)item.ProjectId!))
            throw new NotAuthorizedException("Пользователь не состоит в проекте");

        var comments = commentRepository.GetByItemId(itemId);

        return comments.Select(CommentMapper.ToModel).ToList();
    }
}