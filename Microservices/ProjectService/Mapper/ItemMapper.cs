using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper;

public class ItemMapper
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
            StatusId = (int)item.StatusId,
            IsArchived = item.IsArchived,
        };
    }

    public static ItemModel? ToModel(ItemEntity item)
    {
        if (item is null)
            return null;

        return new ItemModel
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
            Status = StatusMapper.ToModel(item.Status)
        };
    }
}