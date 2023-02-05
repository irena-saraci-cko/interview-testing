namespace PaymentGateway.Application.Dtos.GetPayment
{
    public record GetPaymentApiResponse
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