using PaymentGateway.Application.Dtos.Validators;
using AutoFixture;
using PaymentGateway.Application.Dtos.CreatePayment;
using Shouldly;
using PaymentGateway.Domain.Helpers;

namespace PaymentGateway.Application.Tests.Validators
{
    public class CreatePaymentRequestDtoValidatorTests
    {
        private Fixture _fixture;
        private CreatePaymentRequestDtoValidator _sut;
        private CreatePaymentRequestDto _createPaymentRequestDto;
        public CreatePaymentRequestDtoValidatorTests()
        {
            _fixture = new Fixture();
            _sut = new CreatePaymentRequestDtoValidator();
            _createPaymentRequestDto = _fixture.Build<CreatePaymentRequestDto>()
                .With(r => r.CardNumber, "2222405343248877")
                .With(r => r.ExpiryMonth, 1)
                .With(r => r.ExpiryYear, 2029)
                .With(r => r.Currency, "EUR")
                .With(r => r.Amount, 100)
                .With(r => r.Cvv, "100")
                .Create();
        }

        [Fact]
        public void Given_Valid_CreatePaymentRequest_When_Validator_Is_Invoked_Then_Should_Return_True()
        {
            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeTrue();
        }

        [Theory]
        [InlineData(-1)]
        public void Given_Negative_Amount_When_Validator_Is_Invoked_Then_Should_Return_False(long amount)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Amount = amount };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.AmountInvalid))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        public void Given_Missing_CardNumber_When_Validator_Is_Invoked_Then_Should_Return_False(string cardNumber)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { CardNumber = cardNumber };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CardNumberRequired))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("222240534324887XL")]
        public void Given_CardNumber_With_Letters_When_Validator_Is_Invoked_Then_Should_Return_False(string cardNumber)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { CardNumber = cardNumber };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CardNumberInvalidFormat))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("2222405")]
        [InlineData("222240534324887755555")]
        public void Given_CardNumber_Invalid_Length_When_Validator_Is_Invoked_Then_Should_Return_False(string cardNumber)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { CardNumber = cardNumber };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CardNumberInvalidLength))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData(15)]
        [InlineData(-2)]
        public void Given_Expiry_Month_Invalid_When_Validator_Is_Invoked_Then_Should_Return_False(int month)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { ExpiryMonth = month };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.ExpiryMonthInvalid))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData(1, 2023)]
        [InlineData(1, 2020)]
        public void Given_Expiry_Date_Invalid_When_Validator_Is_Invoked_Then_Should_Return_False(int month, int year)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { ExpiryMonth = month, ExpiryYear = year };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.ExpiryDateInvalid))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("USD")]
        public void Given_Currency_Not_Supported_When_Validator_Is_Invoked_Then_Should_Return_False(string currency)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Currency = currency };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CurrencyInvalid))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        public void Given_Currency_Required_When_Validator_Is_Invoked_Then_Should_Return_False(string currency)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Currency = currency };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CurrencyRequired))
                    .ShouldBeTrue();
        }


        [Theory]
        [InlineData("EURO")]
        public void Given_Currency_Invalid_When_Validator_Is_Invoked_Then_Should_Return_False(string currency)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Currency = currency };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CurrencyInvalidLength))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        public void Given_Cvv_Missing_When_Validator_Is_Invoked_Then_Should_Return_False(string cvv)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Cvv = cvv };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CvvRequired))
                    .ShouldBeTrue();
        }

        [Theory]
        [InlineData("22226")]
        [InlineData("22")]
        public void Given_Cvv_Invalid_When_Validator_Is_Invoked_Then_Should_Return_False(string cvv)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Cvv = cvv };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CvvInvalidLength))
                    .ShouldBeTrue();
        }

         [Theory]
        [InlineData("XYZ")]
        [InlineData("22X")]
        public void Given_Cvv_Invalid_Format_When_Validator_Is_Invoked_Then_Should_Return_False(string cvv)
        {
            // Given
            _createPaymentRequestDto = _createPaymentRequestDto with { Cvv = cvv };

            // When
            var result = _sut.Validate(_createPaymentRequestDto);

            // Then
            result.IsValid.ShouldBeFalse();
            result.Errors
                .Exists(e => e.ErrorMessage.Equals(ErrorCodes.CvvInvalidFormat))
                    .ShouldBeTrue();
        }


    }
}