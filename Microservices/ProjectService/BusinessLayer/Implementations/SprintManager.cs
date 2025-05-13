using Microsoft.AspNetCore.Http;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations
{
    public class SprintManager : ISprintManager
    {
        private readonly ISprintRepository sprintRepository;
        private readonly IBoardRepository boardRepository;
        private readonly IUserProjectRepository userProjectRepository;
        private readonly IItemRepository itemRepository;
        private readonly IAuth auth;

        public SprintManager(ISprintRepository sprintRepository, 
            IAuth auth, 
            IUserProjectRepository userProjectRepository, 
            IBoardRepository boardRepository,
            IItemRepository itemRepository)
        {
            this.sprintRepository = sprintRepository;
            this.auth = auth;
            this.userProjectRepository = userProjectRepository;
            this.boardRepository = boardRepository;
            this.itemRepository = itemRepository;
        }

        public async Task AddItem(int sprintId, int itemId)
        {
            var existingSprint = await sprintRepository.GetByIdAsync(sprintId);

            if (existingSprint is null)
                throw new SprintNotFoundException();

            var existingItem = await itemRepository.GetByIdAsync(itemId);

            if (existingItem is null)
                throw new TaskNotFoundException();

            if (existingItem.ProjectId != existingSprint.Board.ProjectId)
                throw new DifferentAreaException();

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingSprint.Board.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");

            await sprintRepository.AddItem(sprintId, itemId);
        }

        public async Task<int?> CreateAsync(SprintModel statusModel)
        {
            var existingBoard = await boardRepository.GetByIdAsync(statusModel.BoardId);

            if (existingBoard is null)
                throw new BoardNotFoundException();

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingBoard.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");

            var sprintEntity = SprintMapper.ToEntity(statusModel);

            await sprintRepository.CreateAsync(sprintEntity);

            return sprintEntity.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var existingSprint = await sprintRepository.GetByIdAsync(id);

            if (existingSprint is null)
                throw new SprintNotFoundException();

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingSprint.Board.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");

            await sprintRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SprintModel>> GetByBoardIdAsync(int boardId)
        {
            var existingBoard = await boardRepository.GetByIdAsync(boardId);

            if (existingBoard is null)
                throw new BoardNotFoundException();

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingBoard.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");

            var entities = await sprintRepository.GetByBoardId(boardId);

            return entities.Select(SprintMapper.ToModel);
        }

        public async Task<SprintModel> GetByIdAsync(int id)
        {
            var existingSprint = await sprintRepository.GetByIdAsync(id);

            if (existingSprint is null)
                throw new SprintNotFoundException();

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingSprint.Board.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");

            var sprint = await sprintRepository.GetByIdAsync(id);

            return SprintMapper.ToModel(sprint);
        }

        public async Task<int?> UpdateAsync(SprintModel statusModel)
        {
            var existingSprint = await sprintRepository.GetByIdAsync(statusModel.Id);

            if (existingSprint is null)
                throw new SprintNotFoundException();

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId != -1)
                throw new NotAuthorizedException();

            if (!await userProjectRepository.IsUserInProject((int)userId, existingSprint.Board.ProjectId))
                throw new NotAuthorizedException("Текущий пользователь не состоит в проекте");

            var entity = SprintMapper.ToEntity(statusModel);

            await sprintRepository.UpdateAsync(entity);

            return entity.Id;
        }
    }
}
