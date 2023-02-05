namespace PaymentGateway.Application.Dtos.CreatePayment
{
    public record CreatePaymentResponseDto(Guid Id, string Status, string MaskedCardNumber, int ExpiryMonth, int ExpiryYear, string Currency, long Amount);
}