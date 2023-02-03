namespace PaymentGateway.BankAcquirer.Dtos
{
    public class CreatePaymentAcquirerResponse
    {
        public bool Authorized { get; set; }
        public string AuthorizationCode { get; set; } = string.Empty;
    }
}