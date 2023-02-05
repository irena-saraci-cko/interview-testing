using AutoFixture;
using AutoMapper;
using Moq;

using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.Application.Service;
using PaymentGateway.BankAcquirer.Services;
using PaymentGateway.BankAcquirer.Dtos;

using Logger = Serilog.ILogger;
using Shouldly;
using PaymentGateway.Common.ServiceResponses;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Tests.Services
{
    public class PaymentProcessorServiceTests
    {
        private Fixture _fixture;
        private PaymentProcessorService _sut;
        private Mock<IMapper> _mapper = new Mock<IMapper>();
        private Mock<IAcquirerService> _acquirerService = new Mock<IAcquirerService>();
        private Mock<Logger> _logger = new();
        private CreatePaymentApiRequest _paymentRequest;
        private CreatePaymentAcquirerResponse _acquirerResponse;
        private Payment _payment;
        private Guid _paymentId;
        public PaymentProcessorServiceTests()
        {
            _fixture = new Fixture();
            _sut = new PaymentProcessorService(_mapper.Object, _acquirerService.Object, _logger.Object);

            _paymentRequest = _fixture.Create<CreatePaymentApiRequest>();
            _payment = _fixture.Build<Payment>().Create();
            _acquirerResponse = new CreatePaymentAcquirerResponse(true, _fixture.Create<string>());
            _paymentId = _fixture.Create<Guid>();

            _mapper.Setup(m => m.Map<CreatePaymentApiRequest, Payment>(_paymentRequest))
                .Returns(_payment);

            _mapper.Setup(m => m.Map<Payment>(_paymentRequest))
                .Returns(_payment);

            _mapper.Setup(m => m.Map<CreatePaymentApiResponse>(_payment))
                .Returns(_fixture.Create<CreatePaymentApiResponse>());
        }

        [Fact]
        public async Task Given_Acquirer_Authorized_Payment_When_Create_Payment_Then_Should_Return_Payment_Response()
        {
            //Given
            _acquirerService.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>()))
                .ReturnsAsync(_acquirerResponse);

            // When
            var result = await _sut.CreatePayment(_paymentRequest);

            // Then
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_Not_Authorized_Payment_When_Create_Payment_Then_Should_Return_Payment_Response()
        {
            //Given
            _acquirerService.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>()))
                .ReturnsAsync(_acquirerResponse with { Authorized = false });

            // When
            var result = await _sut.CreatePayment(_paymentRequest);

            // Then
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_BadRequest_When_Create_Payment_Then_Should_Return_Validation_Error()
        {
            //Given
            _acquirerService.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>()))
                .ReturnsAsync(new ValidationError("Rejected", "acquire_invalid_data"));

            // When
            var result = await _sut.CreatePayment(_paymentRequest);

            // Then
            result.IsValidationError.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_ServerError_When_Create_Payment_Then_Should_Return_BadGateway_Error()
        {
            //Given
            _acquirerService.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>()))
                .ReturnsAsync(new ServerError());

            // When
            var result = await _sut.CreatePayment(_paymentRequest);

            // Then
            result.IsBadGatewayError.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_TimeoutError_When_Create_Payment_Then_Should_Return_Timeout_Error()
        {
            //Given
            _acquirerService.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>()))
                .ReturnsAsync(new TimeoutError());

            // When
            var result = await _sut.CreatePayment(_paymentRequest);

            // Then
            result.IsTimeoutError.ShouldBeTrue();
        }

        [Theory]
        [InlineData("3fa85f64-5717-4562-b3fc-2c963f66afa6")]
        public async Task Given_Existing_Payment_When_Get_Payment_Then_Should_Return_Payment_Response(string id)
        {
            //Given
            _paymentId = new Guid(id);

            // When
            var result = await _sut.GetPayment(_paymentId);

            // Then
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Non_Existing_Payment_When_Get_Payment_Then_Should_Return_Not_Found_Error()
        {
            // When
            var result = await _sut.GetPayment(_paymentId);

            // Then
            result.IsNotFoundError.ShouldBeTrue();
        }
    }
}