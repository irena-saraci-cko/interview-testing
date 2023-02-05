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
        public async Task<IActionResult> Post(CreatePaymentRequestDto paymentRequest)
        {
            var result = await _paymentProcessorService.CreatePayment(paymentRequest);

            return result.Match<IActionResult>(
                success => Created($"payments/{success.Id}", success),
                validationError => new UnprocessableEntityObjectResult(validationError),
                serverError => StatusCode(500),
                BadGatewayError => StatusCode(502),
                timeoutError => StatusCode(504)
            );

        }
    }
}