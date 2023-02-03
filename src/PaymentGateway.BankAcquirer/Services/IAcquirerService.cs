using PaymentGateway.BankAcquirer.Dtos;
namespace PaymentGateway.BankAcquirer.Services
{
    public interface IAcquirerService
    {
         Task<CreatePaymentAcquirerResponse> CreatePayment(CreatePaymentAcquirerRequest paymentRequest);
    }
}