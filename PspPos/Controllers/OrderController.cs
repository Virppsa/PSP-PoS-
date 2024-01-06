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
    // Create an order ~
    // Get an order +
    // Update an order (merge with update status and assign employee)
    // Delete an order 

    // DISCOUNTS:
    // Create order discount?
    
    // Get ItemOrders
    // Create ItemOrder
    // Get specific ItemOrder
    // Delete ItemOrder
    // Update ItemOrder (merge status and assign)

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Order>>> GetAllOrders([Required]int companyId)
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
    public async Task<ActionResult<Order>> CreateOrder([Required]int companyId, [Required] OrderPostModel order)
    {
        //var createdCompany = _mapper.Map<Or>(company);
        var createdOrder = new Order() { CompanyId = companyId };
        await _orderService.Add(companyId, createdOrder);

        return Ok(createdOrder);
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Company>> GetOrder(int companyId, int orderId)
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

    //[HttpPut("{companyId}")]
    //public async Task<ActionResult<Company>> UpdateCompany(int companyId, CompanyPostModel company)
    //{
    //    var companyUpdateWith = _mapper.Map<Company>(company);
    //    companyUpdateWith.Id = companyId;

    //    var updatedCompany = await _companyService.Update(companyUpdateWith);

    //    if (updatedCompany == null)
    //    {
    //        return NotFound();
    //    }

    //    return Ok(updatedCompany);
    //}

    [HttpDelete("{orderId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteCompany(int companyId, int orderId)
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
}