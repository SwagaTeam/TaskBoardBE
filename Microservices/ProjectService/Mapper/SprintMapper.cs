using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper
{
    public static class SprintMapper
    {
        public static SprintEntity? ToEntity(SprintModel model)
        {
            if (model is null)
                return null;

            return new SprintEntity
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                BoardId = model.BoardId
            };
        }

        public static async Task<SprintModel?> ToModel(SprintEntity model)
        {
            if (model is null)
                return null;

            var itemModels =  await Task.WhenAll(model.Items.Select(ItemMapper.ToModel));


            return new SprintModel
            {
                Id = model.Id,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                BoardId = model.BoardId,
                Items = itemModels
            };
        }
    }
}
