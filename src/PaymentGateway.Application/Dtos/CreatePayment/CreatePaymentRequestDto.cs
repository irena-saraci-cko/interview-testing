namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public record CreatePaymentRequestDto
    {
        public string CardNumber { get; init; } = string.Empty;
        public int ExpiryMonth { get; init; }
        public int ExpiryYear { get; init; }
        public string Currency { get; init; } = string.Empty;
        public long Amount { get; init; }
        public string Cvv { get; init; } = string.Empty;
    }
}