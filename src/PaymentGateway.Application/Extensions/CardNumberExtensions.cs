using System.Text.RegularExpressions;

namespace PaymentGateway.Application.Extensions
{
    public static class CardNumberExtensions
    {
        public static string ToMaskNumber(this string cardNumber)
        {
            var cardLength = cardNumber.Length;
            var maskedString = "XXXX";
            if (cardLength > 4)
            {
                //var firstDigits = cardNumber.Substring(0, 6);
                var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

                var requiredMask = new String('X', cardNumber.Length - 4);

                maskedString = string.Concat(requiredMask, lastDigits);
            }

            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");
            return maskedCardNumberWithSpaces;
        }
    }
}