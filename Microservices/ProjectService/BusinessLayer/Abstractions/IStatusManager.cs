using ProjectService.Models;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IStatusManager
{
    public Task<IEnumerable<StatusModel>> GetAllAsync();
    public Task<IEnumerable<StatusModel>> GetByBoardIdAsync(int id);
    public Task<StatusModel> GetByIdAsync(int id);
    public Task<int?> CreateAsync(StatusModel statusModel);
    public Task<int?> UpdateAsync(StatusModel statusModel);
    public Task ChangeStatusOrderAsync(UpdateOrderModel updateOrderModel);
    public Task DeleteAsync(int id);
}