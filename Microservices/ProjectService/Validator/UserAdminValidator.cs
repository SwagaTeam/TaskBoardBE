using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Services.MailService;

namespace ProjectService.Validator;

public class UserAdminValidator : AbstractValidator<string>
{

    public UserAdminValidator(IUserProjectManager userProjectManager, int? userId, int? projectId)
    {
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserAdmin(userProjectManager, userId, projectId, cancellation))
            .WithMessage("У пользователя недостаточно прав");
    }
}