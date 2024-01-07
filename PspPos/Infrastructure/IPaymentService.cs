﻿using PspPos.Models;

namespace PspPos.Infrastructure;

public interface IPaymentService
{
    public Task<Payment> GetAsync(Guid companyId, Guid paymentId);
    public Task<Payment> CreateAsync(Guid companyId, Guid orderId, Payment payment);
    public Task<Payment> RefundAsync(Guid companyId, Guid paymentId);
}
