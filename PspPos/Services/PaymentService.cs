﻿using PspPos.Commons;
using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;

namespace PspPos.Services;

public class PaymentService: IPaymentService
{
    private readonly ApplicationContext _context;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;

    public PaymentService(ApplicationContext context, IOrderService orderService, IUserService userService)
    {
        _context = context;
        _orderService = orderService;
        _userService = userService;
    }

    public async Task<Payment> CreateAsync(Guid companyId, PaymentPostModel paymentRequest)
    {
        var order = await _orderService.Get(companyId, paymentRequest.OrderId);

        if (order.PaymentId is not null)
            throw new BadHttpRequestException($"Order with id={order.Id} has already processed payment!");

        User user = await _userService.GetUserByCompanyAndUserID(companyId, paymentRequest.CustomerId);

        if (user.LoyaltyPoints < paymentRequest.LoyaltyPointsToUse)
            throw new BadHttpRequestException($"Insufficient loyalty points for user");

        double calculatedLoyaltyDiscount = 0;
        if (paymentRequest.LoyaltyPointsToUse is not 0)
        {
            paymentRequest.LoyaltyPointsToUse = (int)Math.Min(paymentRequest.LoyaltyPointsToUse, order.TotalAmount * 100);

            user.LoyaltyPoints -= paymentRequest.LoyaltyPointsToUse; 
            await _userService.UpdateUser(user.GUID, companyId, new UserPostModel { LoyaltyPoints = user.LoyaltyPoints});
            calculatedLoyaltyDiscount = paymentRequest.LoyaltyPointsToUse / 100;
        }

        Payment payment = new Payment 
        { 
        Id = Guid.NewGuid(),
        LoyaltyDiscount = calculatedLoyaltyDiscount,
        OrderId = paymentRequest.OrderId,
        CustomerId = paymentRequest.CustomerId,
        PaymentMethod = paymentRequest.PaymentMethod,
        PaymentStatus = "Completed",
        // TODO 
        // cap here
        AmountPaid = paymentRequest.AmountPaid - calculatedLoyaltyDiscount,
        };

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
