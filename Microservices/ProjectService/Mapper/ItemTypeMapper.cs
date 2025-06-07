using SharedLibrary.Constants;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.Mapper;

public static class ItemTypeMapper
{
    public static ItemTypeEntity? ToEntity(ItemTypeModel itemType)
    {
        if (itemType == null)
            return null;

        return new ItemTypeEntity
        {
            Level = itemType.Level,
        };
    }
    
    public static ItemTypeModel? ToModel(ItemTypeEntity itemType)
    {
        if (itemType == null)
            return null;

        return new ItemTypeModel
        {
            Id = itemType.Id,
            Level = itemType.Level,
        };
    }
}