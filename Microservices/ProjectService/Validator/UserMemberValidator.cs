using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Services.MailService;
using SharedLibrary.Auth;

namespace ProjectService.Validator;

public class UserMemberValidator : AbstractValidator<string>
{
    public UserMemberValidator(IUserProjectManager userProjectManager, int? userId, int? projectId)
    {
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserCanViewAsync(userProjectManager, userId, projectId, cancellation))
            .WithMessage("Текущий пользователь не находится в нужном проекте");
    }
}