namespace PaymentGateway.Common.ServiceResponses
{
    public record ValidationError(string PaymentStatus, string ErrorCode, List<string>? ErrorMessages = default);
}