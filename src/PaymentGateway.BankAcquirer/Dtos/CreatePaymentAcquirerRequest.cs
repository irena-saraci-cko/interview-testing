namespace PaymentGateway.BankAcquirer.Dtos
{
    public record CreatePaymentAcquirerRequest(string CardNumber, string ExpiryDate, string Currency, long Amount, int Cvv);
}