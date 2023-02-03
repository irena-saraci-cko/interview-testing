namespace PaymentGateway.Domain.Helpers
{
    public static class ErrorCodes
    {
        public const string CardNumberRequired = "card_number_required";
        public const string CardNumberInvalidLength = "card_number_must_be_between_14_and_19_characters_long";
        public const string CardNumberInvalidFormat = "card_number_must_contain_only_numbers";
        public const string CardNumberInvalid = "invalid_credit_card";
        public const string ExpiryDateInvalid = "expiry_data_must_be_in_the_future";
        public const string ExpiryMonthInvalid = "expiry_month_must_be_between_1_and_12";
        public const string ExpiryMonthRequired = "expiry_month_required";
        public const string ExpiryYearRequired = "expiry_year_required";
        public const string CurrencyRequired = "currency_required";
        public const string CurrencyInvalidLength = "currency_must_be_3_characters_long";
        public const string CurrencyInvalid = "currency_invalid";
        public const string AmountRequired = "amount_required";
        public const string AmountInvalid = "amount_must_be_greater_than_0";
        public const string CvvRequired = "cvv_required";
        public const string CvvInvalidLength = "cvv_must_be_between_3_and_4_characters_long";
        public const string CvvInvalidFormat = "cvv_must_contain_only_numbers";
    }
}