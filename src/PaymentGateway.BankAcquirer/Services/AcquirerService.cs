using PaymentGateway.BankAcquirer.Dtos;

namespace PaymentGateway.BankAcquirer.Services
{
    public class AcquirerService : IAcquirerService
    {
        public async Task<CreatePaymentAcquirerResponse> CreatePayment(CreatePaymentAcquirerRequest paymentRequest)
        {
            return new CreatePaymentAcquirerResponse
            {
                AuthorizationCode = "",
                Authorized = true
            };
        }
    }
}