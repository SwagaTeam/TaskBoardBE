using ProjectService.Models;

namespace ProjectService.Validator;

public interface ICreateItemValidator
{
    public Task CheckValidAsync(CreateItemModel createItemModel, CancellationToken token);
}