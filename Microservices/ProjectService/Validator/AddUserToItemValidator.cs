using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;

namespace ProjectService.Validator;

public class AddUserToItemValidator : AbstractValidator<UsersInProjectModel>
{
    private IUserProjectManager userProjectManager;
    public AddUserToItemValidator(IUserProjectManager userProjectManager)
    {
        this.userProjectManager = userProjectManager;
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                IsCurrentUserInProject(model.CurrentUserId, model.ProjectId, cancellation))
            .WithMessage("Текущий пользователь не находится в нужном проекте");

        RuleFor(x => x)
            .MustAsync((model, cancellation) => 
                IsNewUserInProject(model.NewUserId, model.ProjectId, cancellation))
            .WithMessage("Новый пользователь не находится в нужном проекте");
    }

    private async Task<bool> IsCurrentUserInProject(int? currentUserId, int? projectId, CancellationToken cancellation)
    {
        if (projectId == null || projectId == -1 || currentUserId is null || currentUserId == -1) return false;
        return await userProjectManager.IsUserInProjectAsync((int)currentUserId, (int)projectId);
    }

    private async Task<bool> IsNewUserInProject(int newUserId, int? projectId, CancellationToken cancellation)
    {
        if (projectId == null || projectId == -1) return false;
        return await userProjectManager.IsUserInProjectAsync(newUserId, (int)projectId);
    }
}