using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PspPos.Commons;
using PspPos.Infrastructure;
using PspPos.Models;
using System.ComponentModel.DataAnnotations;

namespace PspPos.Controllers;

[ApiController]
[Route("cinematic/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public PaymentController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    // Get payment - create payment, set to finished, throw error if not valid
    // Refund payment - reset order and payment status
    // Get receipt, probably from order

    [HttpPut("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Order>> UpdateOrder([Required] Guid companyId, [Required] Guid orderId, [Required] OrderPostModel order)
    {
        try
        {
            return Ok(await _orderService.Update(companyId, orderId, order));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
