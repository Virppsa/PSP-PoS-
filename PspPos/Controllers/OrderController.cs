using AutoMapper;
using PspPos.Infrastructure;
using PspPos.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using PspPos.Commons;

namespace PspPos.Controllers;

[ApiController]
[Route("cinematic/[controller]")]
public class OrderController : ControllerBase
{

    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    // ORDERS:
    // Get all orders +
    // Create an order +
    // Get an order +
    // Update an order (merge with update status and assign employee) ~
    // Delete an order +

    // DISCOUNTS:
    // Create order discount?
    
    // Get ItemOrders
    // Create ItemOrder
    // Get specific ItemOrder
    // Delete ItemOrder
    // Update ItemOrder (merge status and assign)

    // TODO:
    // get total price and tax and discount

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Order>>> GetAllOrders([Required]Guid companyId)
    {
        try
        {
            return Ok(await _orderService.GetAll(companyId));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Order>> CreateOrder([Required] Guid companyId, [Required] OrderPostModel order)
    {
        try
        {
            var mappedOrder = _mapper.Map<Order>(order);
            return Ok(await _orderService.Add(companyId, mappedOrder));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Company>> GetOrder([Required]Guid companyId, [Required] Guid orderId)
    {
        try
        {
            return Ok(await _orderService.Get(companyId, orderId));
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Order>> UpdateOrder([Required] Guid companyId, [Required] Guid orderId, [Required] OrderPostModel order)
    {
        try
        {
            var mappedOrder = _mapper.Map<Order>(order);
            return Ok(await _orderService.Update(companyId, orderId, mappedOrder));
        }
        catch (NotFoundException ex) 
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteCompany([Required] Guid companyId, [Required] Guid orderId)
    {
        try
        {
            await _orderService.Delete(companyId, orderId);
            return NoContent();
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    //NAGLIO Help with itemOrders---------------------------------------

    // this is not in the contract
    [HttpPost("{companyId}/itemOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderItemViewModel>> CreateOrderItem([Required] Guid companyId, [Required] OrderItemPostModel order)
    {
        try
        {
            return Ok(_mapper.Map<OrderItemViewModel>(await _orderService.AddItemOrder(companyId, order)));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    //this is not in the contract
    [HttpDelete("{companyId}/itemOrders/{itemOrderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteOrderItem([Required] Guid companyId, [Required] Guid id)
    {
        try
        {
            await _orderService.DeleteItemOrder(companyId, id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{companyId}/itemOrders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderItemViewModel>> GetAllItemOrders([Required] Guid companyId, [Required] Guid storeId)
    {
        try
        {
            return Ok(_mapper.Map<List<OrderItemViewModel>>(await _orderService.GetAllItemOrders(companyId, storeId)));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{companyId}/itemOrders/{itemOrderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderItemViewModel>> GetItemOrder([Required] Guid companyId, [Required] Guid id)
    {
        try
        {
            return Ok(_mapper.Map<OrderItemViewModel>(await _orderService.GetItemOrder(companyId, id)));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // /status and /assign PUT endpoints merged into one
    [HttpPut("{companyId}/itemOrders/{itemOrderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderItemViewModel>> UpdateItemOrder([Required] Guid companyId, [Required] Guid id, [Required] OrderItemPostModel order)
    {
        try
        {
            return Ok(_mapper.Map<OrderItemViewModel>(await _orderService.UpdateItemOrder(companyId, id, order)));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}