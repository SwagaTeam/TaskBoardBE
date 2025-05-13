using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations
{
    public class SprintManager : ISprintManager
    {
        private readonly ISprintRepository sprintRepository;
        private readonly IUserProjectRepository userProjectRepository;
        private readonly IAuth auth;

        public SprintManager(ISprintRepository sprintRepository, IAuth auth, IUserProjectRepository userProjectRepository)
        {
            this.sprintRepository = sprintRepository;
            this.auth = auth;
            this.userProjectRepository = userProjectRepository;
        }

        public async Task AddItem(int sprintId, int itemId)
        {
            var existingSprint = await sprintRepository.GetByIdAsync(sprintId);
            var userId = auth.GetCurrentUserId();

            if (existingSprint is null)
                throw new SprintNotFoundException();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingSprint.Board.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");



            
        }

        public Task<int?> CreateAsync(SprintModel statusModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SprintModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SprintModel> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int?> UpdateAsync(SprintModel statusModel)
        {
            throw new NotImplementedException();
        }
    }
}
