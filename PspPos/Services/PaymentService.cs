using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;

namespace PspPos.Services;

public class PaymentService: IPaymentService
{
    private readonly ApplicationContext _context;
    private readonly IOrderService _orderService;

    public PaymentService(ApplicationContext context, IOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    public async Task<Payment> CreateAsync(Guid companyId, Guid orderId, Payment payment)
    {
        payment.OrderId = orderId;
        var order = await _orderService.Get(companyId, orderId);

        payment.Id = Guid.NewGuid();
        order.PaymentId = payment.Id;
        order.Status = "Completed";
        await _orderService.Update(companyId, order.Id, order);

        await _context.Payments.AddAsync(payment);

        await _context.SaveChangesAsync();

        return payment;
    }

    public async Task<Payment> GetAsync(Guid companyId, Guid paymentId)
    {
        throw new NotImplementedException();
    }

    public async Task<Payment> RefundAsync(Guid companyId, Guid paymentId)
    {
        throw new NotImplementedException();
    }
}
