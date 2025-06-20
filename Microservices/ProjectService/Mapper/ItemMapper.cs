using ProjectService.Models;
using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Entities.UserService;
using SharedLibrary.Models;

namespace ProjectService.Mapper;

public static class ItemMapper
{
    public static ItemEntity? ItemToEntity(ItemModel item)
    {
        if (item is null)
            return null;

        return new ItemEntity
        {
            BusinessId = item.BusinessId,
            ParentId = item.ParentId,
            ProjectId = item.ProjectId,
            ProjectItemNumber = item.ProjectItemNumber,
            Title = item.Title,
            Description = item.Description,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
            StartDate = item.StartDate,
            ExpectedEndDate = item.ExpectedEndDate,
            Priority = item.Priority,
            ItemTypeId = item.ItemTypeId,
            StatusId = (int)item.StatusId!,
            IsArchived = item.IsArchived,
            UserItems = item.UserItems != null 
                ? item.UserItems.Select(UserItemMapper.ToEntity).ToList() 
                : new List<UserItemEntity>()
        };
    }

    public static async Task<ItemModel?> ToModel(ItemEntity? item, IUserRepository userRepository)
    {
        if (item is null)
            return null;

        var model = new ItemModel
        {
            Id = item.Id,
            BusinessId = item.BusinessId,
            ParentId = item.ParentId,
            ProjectId = item.ProjectId,
            ProjectItemNumber = item.ProjectItemNumber,
            Title = item.Title,
            Description = item.Description,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
            StartDate = item.StartDate,
            ExpectedEndDate = item.ExpectedEndDate,
            Priority = item.Priority,
            ItemTypeId = item.ItemTypeId,
            StatusId = item.StatusId,
            IsArchived = item.IsArchived,
            Status = StatusMapper.ToModel(item.Status),
            UserItems = item.UserItems.Select(UserItemMapper.ToModel).ToList()
        };

        if(item.ItemsBoards.Count > 0)
        {
            var boardId = item.ItemsBoards.First(x => x.ItemId == model.Id).BoardId;

            model.SetBoardId(boardId);
        }

        if(item.UserItems != null && item.UserItems.Count > 0)
        {
            foreach (var userItem in item.UserItems)
            {
                var user = await userRepository.GetUserAsync(userItem.UserId);
                var contributorModel = new ContributorModel(user.Username, user.ImagePath);
                model.AddContributor(contributorModel);
            }
        }

        var author = await userRepository.GetUserAsync((int)item.AuthorId!);
        model.SetAuthor(author!.Username);

        return model;
    }
}