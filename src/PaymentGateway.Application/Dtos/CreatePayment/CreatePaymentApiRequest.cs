namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public record CreatePaymentApiRequest
    {
        public string CardNumber { get; init; } = string.Empty;
        public int ExpiryMonth { get; init; }
        public int ExpiryYear { get; init; }
        public string Currency { get; init; } = string.Empty;
        public long Amount { get; init; }
        public string Cvv { get; init; } = string.Empty;

        public string ExpiryDate()
        {
            var expiryMonth = ExpiryMonth > 9 ? ExpiryMonth.ToString() : "0" + ExpiryMonth.ToString();
            return $"{expiryMonth}/{ExpiryYear}";
        }
    }
}