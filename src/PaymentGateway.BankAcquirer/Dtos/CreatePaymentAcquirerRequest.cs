namespace PaymentGateway.BankAcquirer.Dtos
{
    public class CreatePaymentAcquirerRequest
    {
        public string CardNumber { get; init; } = string.Empty;
        public string ExpiryDate { get; init; } = string.Empty;
        public string Currency { get; init; } = string.Empty;
        public long Amount { get; init; }
        public string Cvv { get; init; } = string.Empty;
    }
}