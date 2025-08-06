using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s27400_APBD_Project.PaymentPart.DTOs;
using s27400_APBD_Project.PaymentPart.Services;

namespace s27400_APBD_Project.PaymentPart.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("pay")]
    public async Task<IActionResult> PayForContract(PaymentDTO payment, CancellationToken token)
    {
        return Ok(await _paymentService.CreatePayment(payment, token));
    }
}