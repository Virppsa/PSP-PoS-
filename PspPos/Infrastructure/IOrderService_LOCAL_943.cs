using PspPos.Models;

namespace PspPos.Infrastructure;

public interface IOrderService
{
    public Task<Order> Add(Guid companyId, Order order);
    public Task<Order> Get(Guid companyId, Guid id);
    public Task<List<Order>> GetAll(Guid companyId);
    public Task<bool> Delete(Guid companyId, Guid id);
    public Task<Order> Update(Guid companyId, Guid orderId, Order order);
    public Task UpdatePaymentInfo(Guid companyId, Guid orderId, PaymentPostModel payment);
    public Task<string> GetReceipt(Guid companyId, Guid orderId);
}
