using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class SprintManager : ISprintManager
{
    private readonly ISprintRepository sprintRepository;
    private readonly IBoardRepository boardRepository;
    private readonly IUserProjectRepository userProjectRepository;
    private readonly IItemRepository itemRepository;
    private readonly IAuth auth;
    private readonly IValidatorManager _validatorManager;

    public SprintManager(ISprintRepository sprintRepository, IAuth auth, IUserProjectRepository userProjectRepository,
        IBoardRepository boardRepository, IItemRepository itemRepository, IValidatorManager validatorManager)
    {
        this.sprintRepository = sprintRepository;
        this.auth = auth;
        this.userProjectRepository = userProjectRepository;
        this.boardRepository = boardRepository;
        this.itemRepository = itemRepository;
        this._validatorManager = validatorManager;
    }

    public async Task AddItem(int sprintId, int itemId)
    {
        var existingSprint = await sprintRepository.GetByIdAsync(sprintId);
        
        if (existingSprint is null)
            throw new SprintNotFoundException();

        var existingItem = await itemRepository.GetByIdAsync(itemId);

        if (existingItem is null)
            throw new ItemNotFoundException();

        if (existingItem.ProjectId != existingSprint.Board.ProjectId)
            throw new DifferentAreaException();
        await _validatorManager.ValidateUserInProjectAsync(existingSprint.Board.ProjectId);

        await sprintRepository.AddItem(sprintId, itemId);
    }

    public async Task<int?> CreateAsync(SprintModel statusModel)
    {
        var existingBoard = await boardRepository.GetByIdAsync(statusModel.BoardId);

        if (existingBoard is null)
            throw new BoardNotFoundException();

        await _validatorManager.ValidateUserInProjectAsync(existingBoard.ProjectId);

        var sprintEntity = SprintMapper.ToEntity(statusModel);

        await sprintRepository.CreateAsync(sprintEntity);

        return sprintEntity.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var existingSprint = await sprintRepository.GetByIdAsync(id);

        if (existingSprint is null)
            throw new SprintNotFoundException();
        
        await _validatorManager.ValidateUserInProjectAsync(existingSprint.Board.ProjectId);
        
        await sprintRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<SprintModel>> GetByBoardIdAsync(int boardId)
    {
        var existingBoard = await boardRepository.GetByIdAsync(boardId);

        if (existingBoard is null)
            throw new BoardNotFoundException();
        
        await _validatorManager.ValidateUserInProjectAsync(existingBoard.ProjectId);

        var entities = await sprintRepository.GetByBoardId(boardId);

        return entities.Select(SprintMapper.ToModel);
    }

    public async Task<SprintModel> GetByIdAsync(int id)
    {
        var existingSprint = await sprintRepository.GetByIdAsync(id);

        if (existingSprint is null)
            throw new SprintNotFoundException();

        await _validatorManager.ValidateUserInProjectAsync(existingSprint.Board.ProjectId);


        var sprint = await sprintRepository.GetByIdAsync(id);

        return SprintMapper.ToModel(sprint);
    }

    public async Task<int?> UpdateAsync(SprintModel statusModel)
    {
        var existingSprint = await sprintRepository.GetByIdAsync(statusModel.Id);

        if (existingSprint is null)
            throw new SprintNotFoundException();

        await _validatorManager.ValidateUserInProjectAsync(existingSprint.Board.ProjectId);

        var entity = SprintMapper.ToEntity(statusModel);

        await sprintRepository.UpdateAsync(entity);

        return entity.Id;
    }
}