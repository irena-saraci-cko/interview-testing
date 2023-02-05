using PaymentGateway.BankAcquirer.Dtos;
using PaymentGateway.Common.ServiceResponses;

namespace PaymentGateway.BankAcquirer.Services
{
    public interface IAcquirerService
    {
         Task<ServiceResponse<CreatePaymentAcquirerResponse>> CreatePayment(CreatePaymentAcquirerRequest paymentRequest);
    }
}