using PspPos.Models;

namespace PspPos.Infrastructure;

public interface IOrderService
{
    public Task<Order> Add(Guid companyId, Order order);
    public Task<Order> Get(Guid companyId, Guid id);
    public Task<List<Order>> GetAll(Guid companyId);
    public Task<bool> Delete(Guid companyId, Guid id);
<<<<<<< HEAD
    public Task<Order> Update(Guid companyId, Guid orderId, Order order);
    public Task UpdatePaymentInfo(Guid companyId, Guid orderId, PaymentPostModel payment);
    public Task<string> GetReceipt(Guid companyId, Guid orderId);
=======
    public Task<Order> Update(Guid companyId, Guid orderId, OrderPostModel order);
   
    public Task<OrderItem> AddItemOrder(Guid companyId, OrderItemPostModel order);
    public Task<bool> DeleteItemOrder(Guid companyId, Guid id);
    public Task<List<OrderItem>> GetAllItemOrders(Guid companyId, Guid storeId);
    public Task<OrderItem?> GetItemOrder(Guid companyId, Guid id);
    public Task<OrderItem> UpdateItemOrder(Guid companyId, Guid id, OrderItemPostModel order);

>>>>>>> main
}
