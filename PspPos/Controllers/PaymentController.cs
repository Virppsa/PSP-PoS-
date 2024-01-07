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
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;

    public PaymentController(IOrderService orderService, IPaymentService paymentService, IMapper mapper)
    {
        _orderService = orderService;
        _paymentService = paymentService;
        _mapper = mapper;
    }

    // Get payment - create payment, set to finished, throw error if not valid
    // Refund payment - reset order and payment status
    // Get receipt, probably from order

    [HttpPut("{companyId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Payment>> ReceivePayment([Required] Guid companyId, [Required] Guid orderId, [Required] PaymentPostModel paymentModel)
    {
        try
        {
            var payment = _mapper.Map<Payment>(paymentModel);
            return Ok(await _paymentService.CreateAsync(companyId, orderId, payment));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
