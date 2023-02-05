using FluentValidation;
using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.Common.Helpers;

namespace PaymentGateway.Application.Dtos.Validators
{
    public class CreatePaymentRequestDtoValidator : AbstractValidator<CreatePaymentRequestDto>
    {
        private static readonly string[] availableCurrencies = new string[] { "ALL", "EUR", "GBP" };
        private static readonly string numbersOnlyRegex = "^[0-9]*$";
        public CreatePaymentRequestDtoValidator()
        {
            RuleFor(p => p.CardNumber)
            .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ErrorCodes.CardNumberRequired)
                .NotNull()
                    .WithMessage(ErrorCodes.CardNumberRequired)
                .Length(14, 19)
                    .WithMessage(ErrorCodes.CardNumberInvalidLength)
                .Matches(numbersOnlyRegex)
                    .WithMessage(ErrorCodes.CardNumberInvalidFormat)
                .CreditCard()
                    .WithMessage(ErrorCodes.CardNumberInvalid);

            
                RuleFor(p => p.ExpiryMonth)
                                    .Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                    .WithMessage(ErrorCodes.ExpiryMonthRequired)
                                .NotNull()
                                    .WithMessage(ErrorCodes.ExpiryMonthRequired)
                                .InclusiveBetween(1, 12)
                                    .WithMessage(ErrorCodes.ExpiryMonthInvalid)
                        .DependentRules(() =>
                        {
                            RuleFor(p => p.ExpiryYear)
                                .Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                    .WithMessage(ErrorCodes.ExpiryYearRequired)
                                .NotNull()
                                    .WithMessage(ErrorCodes.ExpiryYearRequired)
                            .DependentRules(() =>
                            {
                                RuleFor(p => new { p.ExpiryMonth, p.ExpiryYear })
                                    .Must(date => IsFutureData(date.ExpiryMonth, date.ExpiryYear))
                                        .WithMessage(ErrorCodes.ExpiryDateInvalid);
                            });
                        });


            RuleFor(p => p.Currency)
            .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ErrorCodes.CurrencyRequired)
                .NotNull()
                    .WithMessage(ErrorCodes.CurrencyRequired)
                .Length(3)
                    .WithMessage(ErrorCodes.CurrencyInvalidLength)
                .Must(c => availableCurrencies
                    .Any(p => p.ToLower() == c.ToLower()))
                    .WithMessage(ErrorCodes.CurrencyInvalid);

            RuleFor(p => p.Amount)
            .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ErrorCodes.AmountRequired)
                .NotNull()
                    .WithMessage(ErrorCodes.AmountRequired)
                .GreaterThan(0)
                    .WithMessage(ErrorCodes.AmountInvalid);

            RuleFor(p => p.Cvv)
            .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ErrorCodes.CvvRequired)
                .NotNull()
                   .WithMessage(ErrorCodes.CvvRequired)
                .Length(3, 4)
                    .WithMessage(ErrorCodes.CvvInvalidLength)
                .Matches(numbersOnlyRegex)
                   .WithMessage(ErrorCodes.CvvInvalidFormat);

        }

        private bool IsFutureData(int expiryMonth, int expiryYear)
        {
            //keep the card valid during the last month
            var expiryDate = new DateTime(year: expiryYear, month: expiryMonth + 1, day: 1);

            return expiryDate > DateTime.Today;
        }
    }
}