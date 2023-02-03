namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public class CreatePaymentRequestDto
    {
        public string CardNumber { get; set; } = string.Empty;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Currency { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Cvv { get; set; } = string.Empty;
    }
}