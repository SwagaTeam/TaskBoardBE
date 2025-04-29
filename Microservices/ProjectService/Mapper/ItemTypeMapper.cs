using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper;

public class ItemTypeMapper
{
    public static ItemTypeEntity ToEntity(ItemTypeModel itemType)
    {
        return new ItemTypeEntity
        {
            Id = itemType.Id,
            Level = itemType.Level,
        };
    }
    
    public static ItemTypeModel ToModel(ItemTypeEntity itemType)
    {
        return new ItemTypeModel
        {
            Id = itemType.Id,
            Level = itemType.Level,
        };
    }
}