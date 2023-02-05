namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public record CreatePaymentApiResponse
    {
        public Guid Id { get; set; }
        public string MaskedCardNumber { get; init; } = string.Empty;
        public int ExpiryMonth { get; init; }
        public int ExpiryYear { get; init; }
        public string Currency { get; init; } = string.Empty;
        public long Amount { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}