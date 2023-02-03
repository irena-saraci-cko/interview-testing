using FluentValidation;

using PaymentGateway.Application.Dtos.CreatePayment;

namespace PaymentGateway.Application.Dtos.Validators
{
    public class CreatePaymentRequestDtoValidator : AbstractValidator<CreatePaymentRequestDto>
    {
        private static readonly string[] availableCurrencies = new string[] { "ALL", "EUR", "GBP" };
        private static readonly string numbersOnlyRegex = "^[0-9]*$";
        public CreatePaymentRequestDtoValidator()
        {
            RuleFor(p => p.CardNumber)
                .NotEmpty()
                .NotNull()
                    .WithMessage("card_number_required")
                .Length(14, 19)
                    .WithMessage("card_number_must_be_between_14_and_19_characters_long")
                .Matches(numbersOnlyRegex)
                    .WithMessage("card_number_must_contain_only_numbers")
                .CreditCard()
                    .WithMessage("invalid_credit_card");

            RuleFor(p => new { p.ExpiryMonth, p.ExpiryYear })
                .Must(date => IsFutureData(date.ExpiryMonth, date.ExpiryYear))
                    .WithMessage("expiry_data_must_be_in_the_future")
                        .DependentRules(() =>
                        {
                            RuleFor(p => p.ExpiryMonth)
                            .NotEmpty()
                            .NotNull()
                                .WithMessage("expiry_month_required")
                            .GreaterThanOrEqualTo(1)
                            .LessThanOrEqualTo(12)
                                .WithMessage("expiry_month_must_be_between_1_and_12");

                            RuleFor(p => p.ExpiryYear)
                                .NotEmpty()
                                .NotNull()
                                    .WithMessage("expiry_year_required");
                        });

            RuleFor(p => p.Currency)
                .NotEmpty()
                .Null()
                    .WithMessage("currency_required")
                .Length(3)
                    .WithMessage("currency_must_be_3_characters_long")
                .Must(c => availableCurrencies
                    .Any(p => p.ToLower() == c.ToLower()))
                    .WithMessage("currency_invalid"); ;

            RuleFor(p => p.Amount)
                .NotEmpty()
                .Null()
                    .WithMessage("amount_required")
                .GreaterThan(0)
                    .WithMessage("amount_must_be_greater_than_0");

            RuleFor(p => p.Cvv)
               .NotEmpty()
               .Null()
                   .WithMessage("cvv_required")
               .Length(3, 4)
               .WithMessage("cvv_must_be_between_3_and_4_characters_long")
               .Matches(numbersOnlyRegex)
                   .WithMessage("cvv_must_contain_only_numbers");

        }

        private bool IsFutureData(int expiryMonth, int expiryYear)
        {
            //keep the card valid during the last month
            var expiryDate = new DateTime(month: expiryMonth + 1, year: expiryYear, day: 1);

            return expiryDate > DateTime.Today;
        }
    }
}