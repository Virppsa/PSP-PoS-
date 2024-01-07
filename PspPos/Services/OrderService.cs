using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using PspPos.Commons;
using AutoMapper;

namespace PspPos.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationContext _context;
    private readonly IAppointmentsService _appointmentsService;
    private readonly IMapper _mapper;

    public OrderService(ApplicationContext context, IAppointmentsService appointmentsService, IMapper mapper)
    {
        _context = context;
        _appointmentsService = appointmentsService;
        _mapper = mapper;
    }

    public async Task<Order> Add(Guid companyId, Order order)
    {
        if(!await _context.CheckIfCompanyExists(companyId))
            throw new NotFoundException($"Company with id={companyId} not found");

        order.CompanyId = companyId;

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<bool> Delete(Guid companyId, Guid id)
    {
        var order = await Get(companyId, id);

        _context.Orders.Remove(order);
        _context.SaveChanges();
        return true;
    }

    public async Task<Order> Get(Guid companyId, Guid id)
    {
        var orders = await GetAll(companyId);
        var matchingOrder = orders.FirstOrDefault(o => o.Id == id);

        if (matchingOrder is null)
            throw new NotFoundException($"Item with id={id} not found");

        return matchingOrder;
    }

    public async Task<List<Order>> GetAll(Guid companyId)
    {
        if (!await _context.CheckIfCompanyExists(companyId))
            throw new NotFoundException($"Company with id={companyId} not found");

        var orders = await _context.Orders.ToListAsync();
        return orders.Where(o => o.CompanyId == companyId).ToList();
    }

    public async Task<Order> Update(Guid companyId, Guid orderId, Order order)
    {
        if (!await _context.CheckIfCompanyExists(companyId))
            throw new NotFoundException($"Company with id={companyId} not found");

        var orderToUpdate = await Get(companyId, orderId);

        orderToUpdate.WorkerId = order.WorkerId;
        orderToUpdate.CustomerId = order.CustomerId;
        orderToUpdate.Gratuity = order.Gratuity;

        await AddNewAppointments(orderToUpdate.Id, orderToUpdate.Appointments, order.Appointments);
        await RemoveDeletedAppointments(orderToUpdate.Appointments, order.Appointments);
        orderToUpdate.Appointments = order.Appointments;

        orderToUpdate.ItemOrders = order.ItemOrders;
        orderToUpdate.Status = order.Status;

        await _context.SaveChangesAsync();

        return orderToUpdate;
    }

    // TODO
    // test these out!
    private async Task RemoveDeletedAppointments(Guid[] oldAppointments, Guid[] newAppointments)
    {
        var deletedAppointments = await _appointmentsService.GetAllByPropertyAsync(app => oldAppointments.Contains(app.Id) && !newAppointments.Contains(app.Id));
        foreach (var appointment in deletedAppointments)
        {
            if (!await _appointmentsService.CheckIfAppointmentExists(appointment.Id))
                throw new NotFoundException($"Could not remove appointment with id={appointment.Id} from order as no such appointmentId exists");

            appointment.OrderId = null;
            await _appointmentsService.UpdateAsync(appointment);
        }
    }

    private async Task AddNewAppointments(Guid orderId, Guid[] oldAppointments, Guid[] newAppointments)
    {
        var createdAppointments = await _appointmentsService.GetAllByPropertyAsync(app => !oldAppointments.Contains(app.Id) && newAppointments.Contains(app.Id));
        foreach (var appointment in createdAppointments)
        {
            if (!await _appointmentsService.CheckIfAppointmentExists(appointment.Id))
                throw new NotFoundException($"Could not add appointment with id={appointment.Id} to order with id={orderId} as no such appointmentId exists");

            appointment.OrderId = orderId;
            await _appointmentsService.UpdateAsync(appointment);
        }
    }

    public async Task UpdateTotals()
    {
        // get totals and receipt
    }

    public async Task UpdatePaymentInfo(Guid companyId, Guid orderId, PaymentPostModel payment)
    {
        throw new NotImplementedException();
    }

    public async  Task<string> GetReceipt(Guid companyId, Guid orderId)
    {
        throw new NotImplementedException();
    }

    //NAGLIO Help with itemOrders---------------------------------------
    //Add items to order (id item, store id)
    //remove items from order (orderITEM ID)
  
    public async Task<OrderItem> AddItemOrder(Guid companyId, OrderItemPostModel order)
    {
        if (await _context.CheckIfCompanyExists(companyId) == false)
        {
            throw new NotFoundException($"Company with id={companyId} doesn't exist");
        }
        else
        {
            var itemOrder = _mapper.Map<OrderItem>(order);
            itemOrder.CompanyId = companyId;
            await _context.OrderItems.AddAsync(itemOrder);
            await _context.SaveChangesAsync();
            return itemOrder;
        }
    }

    public async Task<bool> DeleteItemOrder(Guid companyId, Guid id)
    {
        if (await _context.CheckIfCompanyExists(companyId) == false)
        {
            throw new NotFoundException($"Company with id={companyId} doesn't exist");
        }
        else
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null)
            {
                return false; // Not found
            }

            _context.OrderItems.Remove(item);

            await _context.SaveChangesAsync();
            return true; // Successful
        }
    }

    public async Task<List<OrderItem>> GetAllItemOrders(Guid companyId, Guid storeId)
    {
        if (await _context.CheckIfCompanyExists(companyId) == false)
        {
            throw new NotFoundException($"Company with id={companyId} doesn't exist");
        }
        else
        {
            var itemOrders = from itemOrder in await _context.OrderItems.ToListAsync()
                               where itemOrder.CompanyId == companyId && itemOrder.StoreId == storeId
                               select itemOrder;
            return itemOrders.ToList();
        }
    }

    public async Task<OrderItem?> GetItemOrder(Guid companyId, Guid id)
    {
        if (await _context.CheckIfCompanyExists(companyId) == false)
        {
            throw new NotFoundException($"Company with id={companyId} doesn't exist");
        }
        else
        {
            return await _context.OrderItems.FindAsync(id);
        }
    }

    public async Task<OrderItem> UpdateItemOrder(Guid companyId, Guid id, OrderItemPostModel order)
    {
        if (await _context.CheckIfCompanyExists(companyId) == false)
        {
            throw new NotFoundException($"Company with id={companyId} doesn't exist");
        }
        else
        {
            var itemToUpdate = await GetItemOrder(companyId, id);
            if (itemToUpdate == null)
            {
                return null;
            }

            itemToUpdate.ItemId = order.ItemId;
            itemToUpdate.StoreId = order.StoreId;
            itemToUpdate.ItemOptions = order.ItemOptions;
            itemToUpdate.Status = order.Status;
            itemToUpdate.WorkerId = order.WorkerId;

            await _context.SaveChangesAsync();
            return itemToUpdate;
        }
    }
}
