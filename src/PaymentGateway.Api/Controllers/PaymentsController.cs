using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Application.Dtos.CreatePayment;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {

        [HttpPost]
        public async Task<ActionResult<CreatePaymentResponseDto>> Post(CreatePaymentRequestDto paymentRequest)
        {
            return Ok();
        }
    }
}