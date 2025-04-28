using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ItemsService.Mapper;

public class ItemMapper
{
    public static ItemEntity ItemToEntity(ItemModel model)
    {
        return new ItemEntity
        {
            Id = model.Id,
            BusinessId = model.BusinessId,
            CreatedAt = model.CreatedAt,
            Description = model.Description,
        }
    }
}