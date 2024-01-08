using PspPos.Data;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.EntityFrameworkCore;
using PspPos.Commons;
using AutoMapper;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PspPos.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationContext _context;
    private readonly IAppointmentsService _appointmentsService;
    private readonly IServiceService _serviceService;
    private readonly IMapper _mapper;
    private readonly IItemsService _itemsService;

    public OrderService(ApplicationContext context, IAppointmentsService appointmentsService, IServiceService serviceService, IMapper mapper, IItemsService itemsService)
    {
        _context = context;
        _appointmentsService = appointmentsService;
        _serviceService = serviceService;
        _mapper = mapper;
        _itemsService = itemsService;
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

    // TODO
    // test out if you can update finalized order
    public async Task<Order> Update(Guid companyId, Guid orderId, Order order)
    {
        if (!await _context.CheckIfCompanyExists(companyId))
            throw new NotFoundException($"Company with id={companyId} not found");

        var orderToUpdate = await Get(companyId, orderId);
        if (orderToUpdate.Status is "Completed" or "Refunded")
            throw new BadHttpRequestException("Cannot update order which has been finalized");

        orderToUpdate.WorkerId = order.WorkerId;
        orderToUpdate.CustomerId = order.CustomerId;

        // TODO
        // UPDATE RECEIPT HERE!!
        // receipt flow:
        // on update receipt is completely redone, on payment, order level discounts are added and order can no longer be updated

        double newTotalAmount = 0;
        double newTotalTax = 0;
        string newReceipt = $"--- RECEIPT FOR CUSTOMER {orderToUpdate.CustomerId}: ---\n";

        await AddNewAppointments(orderToUpdate.Id, orderToUpdate.Appointments, order.Appointments);
        await RemoveDeletedAppointments(orderToUpdate.Appointments, order.Appointments);
        orderToUpdate.Appointments = order.Appointments;
        var aggregatedAppointmentInfo = await GetTotalsAppointments(companyId, order.Appointments);
        newTotalAmount += aggregatedAppointmentInfo.TotalPrice;
        newTotalTax += aggregatedAppointmentInfo.TotalTax;
        newReceipt += aggregatedAppointmentInfo.PartialReceipt;

        orderToUpdate.ItemOrders = order.ItemOrders;
        orderToUpdate.Status = order.Status;

        orderToUpdate.TotalAmount = newTotalAmount;
        orderToUpdate.Tax = newTotalTax;
        orderToUpdate.Receipt = newReceipt;
        await _context.SaveChangesAsync();

        //Also update OrderItems' orderIds
        foreach (var itemOrderId in orderToUpdate.ItemOrders)
        {
            var itemOrder = await GetItemOrder(companyId, itemOrderId) ?? throw new NotFoundException($"ItemOrder {itemOrderId} doesn't exist");
            itemOrder.OrderId = orderId;
            await UpdateItemOrder(companyId, itemOrderId, _mapper.Map<OrderItemPostModel>(itemOrder));
        }

        return orderToUpdate;
    }

    private async Task<(double TotalPrice, double TotalTax, string PartialReceipt)> GetTotalsAppointments(Guid companyId, Guid[] appointmentIds)
    {
        double totalPrice = 0;
        double totalTax = 0;
        string partialReceipt = "--- APPOINTMENTS: ---\n";

        var allAppointments = (await _appointmentsService.GetAllByPropertyAsync(app => appointmentIds.Contains(app.Id))).ToList();

        var allServices = await _serviceService.GetAllByPropertyAsync(service => service.companyId == companyId); 

        foreach(var app in allAppointments)
        {
            var service = allServices.First(service => service.Id == app.ServiceId);
            ServiceDiscount? discount = null;
            if(service.SerializedDiscount is not null)
            {
                discount = JsonSerializer.Deserialize<ServiceDiscount>(service.SerializedDiscount);
            }
            double discountAmount = discount?.DiscountPercentage ?? 0;

            double priceAfterDiscount = service.Price - (service.Price * discountAmount);
            double taxAmount = priceAfterDiscount * service.Tax;
            double priceAfterTax = priceAfterDiscount + taxAmount;
            partialReceipt += $"+ {service.Name}: {service.Price} ({discountAmount}% DISCOUNT) ({service.Tax}% TAX) = {priceAfterTax}\n";

            totalPrice += priceAfterTax;
            totalTax += taxAmount;
        }

        partialReceipt += "\n";

        return (totalPrice, totalTax, partialReceipt);
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

            //Also, decrease inventory Amount
            foreach (var inventory in await _itemsService.GetAllInventories(companyId))
            {
                if (inventory.ItemId == order.ItemId)
                {
                    inventory.Amount -= 1;
                    await _itemsService.UpdateInventory(inventory);
                }
            }

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
