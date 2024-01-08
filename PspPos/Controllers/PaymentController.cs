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

    [HttpGet("{companyId}/receipt")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<string>> GetReceipt([Required] Guid companyId, [Required] Guid orderId)
    {
        try
        {
            var order = await _orderService.Get(companyId, orderId);
            return Ok(order.Receipt);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{companyId}/receive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Payment>> ReceivePayment([Required] Guid companyId, [Required] PaymentPostModel paymentModel)
    {
        try
        {
            var payment = _mapper.Map<Payment>(paymentModel);
            return Ok(await _paymentService.CreateAsync(companyId, paymentModel));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{companyId}/get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Payment>> GetPayment([Required] Guid companyId, [Required] Guid paymentId)
    {
        try
        {
            // TODO:
            // idk if we should check if payment is valid, what if the cashier is super chill and cool
            // implement api endpoint for super chill and cool cashiers which accepts anything
            return Ok(await _paymentService.GetAsync(companyId, paymentId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{companyId}/refund")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Payment>> RefundPayment([Required] Guid companyId, [Required] Guid paymentId)
    {
        try
        {
            return Ok(await _paymentService.RefundAsync(companyId, paymentId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
