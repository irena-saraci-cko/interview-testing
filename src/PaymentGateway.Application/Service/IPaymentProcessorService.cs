using PaymentGateway.Application.Dtos;
using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.Common.ServiceResponses;

namespace PaymentGateway.Application.Service
{
    public interface IPaymentProcessorService
    {
         Task<ServiceResponse<CreatePaymentResponseDto>> CreatePayment(CreatePaymentRequestDto paymentRequest);
    }
}