using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using SharedLibrary.Auth;

namespace ProjectService.Validator;

public class CreateItemValidator
    : AbstractValidator<CreateItemModel>
{
    private readonly IProjectManager projectManager;
    private readonly IBoardManager boardManager;
    private readonly IAuth authManager;
    
    public CreateItemValidator(IBoardManager boardManager, IProjectManager projectManager, IAuth authManager)
    {
        this.boardManager = boardManager;
        this.projectManager = projectManager;
        this.authManager = authManager;
        
        RuleFor(x => x)
            .MustAsync(IsUserMember)
            .WithMessage("Пользователь не имеет прав на создание задач");

        RuleFor(x => x)
            .MustAsync(BeValidBoard)
            .WithMessage("Неверный boardId");
    }

    private async Task<bool> IsUserMember(CreateItemModel item, CancellationToken cancellation)
    {
        var currentId = authManager.GetCurrentUserId();
        if (currentId is null) return false;

        var a = await projectManager.IsUserViewer((int)currentId, (int)item.Item.ProjectId);
        return a;
    }

    private async Task<bool> BeValidBoard(CreateItemModel item, CancellationToken cancellation)
    {
        var board = await boardManager.GetById(item.BoardId);
        return board != null && board.ProjectId == item.Item.ProjectId;
    }
}