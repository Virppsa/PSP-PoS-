using PspPos.Models;

namespace PspPos.Infrastructure;

public interface IOrderService
{
    public Task<Order> Add(Guid companyId, OrderPostModel order);
    public Task<Order> Get(Guid companyId, Guid id);
    public Task<List<Order>> GetAll(Guid companyId);
    public Task<bool> Delete(Guid companyId, Guid id);
    public Task<Order> Update(Guid companyId, Guid orderId, OrderPostModel order);
   
    public Task<OrderItem> AddItemOrder(Guid companyId, OrderItemPostModel order);
    public Task<bool> DeleteItemOrder(Guid companyId, Guid id);
    public Task<List<OrderItem>> GetAllItemOrders(Guid companyId, Guid storeId);
    public Task<OrderItem?> GetItemOrder(Guid companyId, Guid id);
    public Task<OrderItem> UpdateItemOrder(Guid companyId, Guid id, OrderItemPostModel order);

}
