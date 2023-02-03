namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public record CreatePaymentResponseDto
    {
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        public string MaskedCardNumber { get; init; } = string.Empty;
        public int ExpiryMonth { get; init; }
        public int ExpiryYear { get; init; }
        public string Currency { get; init; } = string.Empty;
        public int Amount { get; init; }

    }
}