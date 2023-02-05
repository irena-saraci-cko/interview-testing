using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Application.Dtos.GetPayment;
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
        public async Task<ActionResult<CreatePaymentApiResponse>> Post(CreatePaymentApiRequest paymentRequest)
        {
            var result = await _paymentProcessorService.CreatePayment(paymentRequest);

            return result.Match<ActionResult<CreatePaymentApiResponse>>(
                success => Created($"payments/{success.Id}", success),
                validationError => new UnprocessableEntityObjectResult(validationError),
                notFoundError => NotFound(),
                serverError => StatusCode(500),
                BadGatewayError => StatusCode(502),
                timeoutError => StatusCode(504)
            );

        }

        [HttpGet]
        [Route("{paymentId}")]
        public async Task<ActionResult<GetPaymentApiResponse>> Get(string paymentId)
        {
            if (Guid.TryParse(paymentId, out Guid id))
            {
                var result = await _paymentProcessorService.GetPayment(id);

                return result.Match<ActionResult<GetPaymentApiResponse>>(
                    success => Ok(success),
                    validationError => new UnprocessableEntityObjectResult(validationError),
                    notFoundError => NotFound(),
                    serverError => StatusCode(500),
                    BadGatewayError => StatusCode(502),
                    timeoutError => StatusCode(504)
                );
            }
            else
            {
                return NotFound();
            }

        }
    }
}