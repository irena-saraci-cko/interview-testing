namespace PaymentGateway.BankAcquirer.Dtos
{
    public record CreatePaymentAcquirerResponse(bool Authorized, string AuthorizationCode);
}