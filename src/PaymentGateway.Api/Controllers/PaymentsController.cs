using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Application.Service;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentProcessorService _paymentProcessorService;
        public PaymentsController(IPaymentProcessorService paymentProcessorService)
        {
            _paymentProcessorService = paymentProcessorService ?? throw new ArgumentNullException(nameof(paymentProcessorService));
        }

        [HttpPost]
        public async Task<ActionResult<CreatePaymentResponseDto>> Post(CreatePaymentRequestDto paymentRequest)
        {
            var result = await _paymentProcessorService.CreatePayment(paymentRequest);
            return Ok(result);
        }
    }
}