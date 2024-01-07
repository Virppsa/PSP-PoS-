using PspPos.Commons;
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

        if (order.PaymentId is not null)
            throw new BadHttpRequestException($"Order with id={order.Id} has already processed payment!");

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
        if(!await _context.CheckIfCompanyExists(companyId))
            throw new NotFoundException($"Company with id={companyId} not found");

        var payment = _context.Payments.FirstOrDefault(p => p.Id == paymentId);
        if(payment is null)
            throw new NotFoundException($"Payment with id={paymentId} not found");

        return payment!;
    }

    public async Task<Payment> RefundAsync(Guid companyId, Guid paymentId)
    {
        var payment = await GetAsync(companyId, paymentId);
        if (payment.PaymentStatus == "Refunded")
            throw new BadHttpRequestException($"Payment with id={paymentId} has already been refunded!");

        payment.PaymentStatus = "Refunded";

        var order = await _orderService.Get(companyId, payment.OrderId);
        order.Status = "Refunded";

        await _context.SaveChangesAsync();

        return payment;
    }
}
