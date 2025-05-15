using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;

namespace ProjectService.Validator;

public class UserInProjectValidator : AbstractValidator<string>
{
    private IUserProjectManager userProjectManager;

    public UserInProjectValidator(IUserProjectManager userProjectManager, int? userId, int? projectId)
    {
        this.userProjectManager = userProjectManager;
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserInProjectAsync(userProjectManager, userId, projectId, cancellation))
            .WithMessage("Текущий пользователь не находится в нужном проекте");
    }
}