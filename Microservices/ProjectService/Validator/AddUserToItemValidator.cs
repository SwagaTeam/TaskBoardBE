using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;

namespace ProjectService.Validator;

public class AddUserToItemValidator : AbstractValidator<UsersInProjectModel>
{
    private readonly IUserProjectManager userProjectManager;
    public AddUserToItemValidator(IUserProjectManager userProjectManager)
    {
        this.userProjectManager = userProjectManager;
        
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserInProjectAsync(
                    userProjectManager, model.CurrentUserId, model.ProjectId, cancellation))
            .WithMessage("Текущий пользователь не находится в нужном проекте");
            
        RuleFor(x => x)
            .MustAsync((model, cancellation) => 
                UserInProjectService.IsUserInProjectAsync(
                    userProjectManager, model.NewUserId, model.ProjectId, cancellation))
            .WithMessage("Новый пользователь не находится в нужном проекте");
    }

    private async Task<bool> IsNewUserInProject(int newUserId, int? projectId)
    {
        if (projectId == null || projectId == -1) return false;
        return await userProjectManager.IsUserInProjectAsync(newUserId, (int)projectId);
    }
}