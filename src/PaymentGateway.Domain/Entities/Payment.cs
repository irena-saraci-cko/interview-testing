using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public PaymentStatus Status { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; init; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string AuthorizationCode { get; set; } = string.Empty;
    }
}