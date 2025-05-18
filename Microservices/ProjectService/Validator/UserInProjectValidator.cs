using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;

namespace ProjectService.Validator;

public class UserInProjectValidator : AbstractValidator<string>
{

    public UserInProjectValidator(IUserProjectManager userProjectManager, int? userId, int? projectId)
    {
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserInProjectAsync(userProjectManager, userId, projectId, cancellation))
            .WithMessage("Проект не найден либо текущий пользователь не имеет полномочий");
    }
}