using FluentValidation;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;

namespace ProjectService.Validator;

public class AddUserToItemValidator : AbstractValidator<UsersInProjectModel>
{

    public AddUserToItemValidator(IUserProjectManager userProjectManager, IItemRepository itemRepository)
    {
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
        
        RuleFor(x => x)
            .MustAsync((model, cancellation) =>
                UserInProjectService.IsUserInItem(itemRepository, model.NewUserId, model.ItemId))
            .WithMessage("Новый пользователь ужн находится в проекте");
    }
}