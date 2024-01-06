using PspPos.Models;

namespace PspPos.Infrastructure;

public interface IOrderService
{
    public Task Add(Guid companyId, Order order);
    public Task<Order> Get(Guid companyId, Guid id);
    public Task<List<Order>> GetAll(Guid companyId);
    public Task<bool> Delete(Guid companyId, Guid id);
    public Task<Order> Update(Guid companyId, Order order);
}
