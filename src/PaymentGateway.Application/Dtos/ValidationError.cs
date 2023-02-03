namespace PaymentGateway.Application.Dtos
{
    public record ValidationError(string PaymentStatus, string ErrorCode, List<string> ErrorMessages);
}