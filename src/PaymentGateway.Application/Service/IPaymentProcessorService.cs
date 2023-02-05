using PaymentGateway.Application.Dtos;
using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.Application.Dtos.GetPayment;
using PaymentGateway.Common.ServiceResponses;

namespace PaymentGateway.Application.Service
{
    public interface IPaymentProcessorService
    {
         Task<ServiceResponse<CreatePaymentApiResponse>> CreatePayment(CreatePaymentApiRequest paymentRequest);
         Task<ServiceResponse<GetPaymentApiResponse>> GetPayment(Guid paymentId);
    }
}