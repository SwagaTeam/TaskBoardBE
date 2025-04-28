using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using SharedLibrary.Auth;

namespace ProjectService.Validator;

public class CreateItemValidator(IBoardManager boardManager, IProjectManager projectManager, IAuth authManager)
    : AbstractValidator<CreateItemModel>, ICreateItemValidator
{
    public async Task CheckValidAsync(CreateItemModel createItemModel, CancellationToken token)
    {
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

        return await projectManager.IsUserViewer((int)currentId, (int)item.Item.ProjectId);
    }

    private async Task<bool> BeValidBoard(CreateItemModel item, CancellationToken cancellation)
    {
        var board = await boardManager.GetById(item.BoardId);
        return board != null && board.ProjectId == item.Item.ProjectId;
    }
}