using PaymentGateway.Application.Dtos.CreatePayment;

namespace PaymentGateway.Application.Service
{
    public interface IPaymentProcessorService
    {
         Task<CreatePaymentResponseDto> CreatePayment(CreatePaymentRequestDto paymentRequest);
    }
}