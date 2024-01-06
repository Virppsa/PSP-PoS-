﻿using PspPos.Models;

namespace PspPos.Infrastructure;

public interface IOrderService
{
    public Task<Order> Add(int companyId, OrderPostModel order);
    public Task<Order> Get(int companyId, int id);
    public Task<List<Order>> GetAll(int companyId);
    public Task<bool> Delete(int companyId, int id);
    public Task<Order> Update(int companyId, int orderId, OrderPostModel order);
}
