namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public record CreatePaymentRequestDto(string CardNumber, int ExpiryMonth, int ExpiryYear, string Currency, long Amount, string Cvv);
}