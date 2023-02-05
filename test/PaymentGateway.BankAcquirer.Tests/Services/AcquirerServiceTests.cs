using Logger = Serilog.ILogger;
using RichardSzalay.MockHttp;
using AutoFixture;
using PaymentGateway.BankAcquirer.Services;
using Shouldly;
using Moq;
using PaymentGateway.BankAcquirer.Dtos;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;

namespace PaymentGateway.BankAcquirer.Tests.Services
{
    public class AcquirerServiceTests
    {
        private MockHttpMessageHandler _mockHttp;
        private Mock<Logger> _logger = new();
        private Fixture _fixture;
        private AcquirerService _sut;
        private CreatePaymentAcquirerResponse _response;
        public AcquirerServiceTests()
        {
            _fixture = new Fixture();
            _mockHttp = new MockHttpMessageHandler();
            _response = _fixture.Create<CreatePaymentAcquirerResponse>();
            var client = _mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/payments");
            _sut = new AcquirerService(client, _logger.Object);
        }

        [Fact]
        public async Task Given_Acquirer_Returns_Ok_When_Sending_Request_To_Acquirer_Bank_Then_Should_Return_Valid_Acquirer_Response()
        {
            //Given
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_response),
                    Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            _mockHttp.When(HttpMethod.Post, "*")
                    .Respond(_ =>
                    {
                        return response;
                    });

            // When
            var result = await _sut.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>());

            // Then
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_Returns_UnSerializable_Response_When_Sending_Request_To_Acquirer_Bank_Then_Should_Return_Server_Error()
        {
            //Given
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject("{}"),
                    Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            _mockHttp.When(HttpMethod.Post, "*")
                    .Respond(_ =>
                    {
                        return response;
                    });

            // When
            var result = await _sut.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>());

            // Then
            result.IsServerError.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_Returns_BadRequest_When_Sending_Request_To_Acquirer_Bank_Then_Should_Return_Validation_Error()
        {
            //Given
            _mockHttp.When(HttpMethod.Post, "*")
                    .Respond(_ =>
                    {
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                    });

            // When
            var result = await _sut.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>());

            // Then
            result.IsValidationError.ShouldBeTrue();
        }


        [Fact]
        public async Task Given_Acquirer_Throws_HttpRequestException_When_Sending_Request_To_Acquirer_Bank_Then_Should_Return_Server_Error()
        {
            //Given
            _mockHttp.When(HttpMethod.Post, "*")
                    .Respond(_ =>
                    {
                        throw new HttpRequestException();
                    });

            // When
            var result = await _sut.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>());

            // Then
            result.IsServerError.ShouldBeTrue();
        }

        [Fact]
        public async Task Given_Acquirer_Throws_TimeoutException_When_Sending_Request_To_Acquirer_Bank_Then_Should_Return_Timeout_Error()
        {
            //Given
            _mockHttp.When(HttpMethod.Post, "*")
                    .Respond(_ =>
                    {
                        throw new TimeoutException();
                    });

            // When
            var result = await _sut.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>());

            // Then
            result.IsTimeoutError.ShouldBeTrue();
        }

         [Fact]
        public async Task Given_Acquirer_Throws_Exception_When_Sending_Request_To_Acquirer_Bank_Then_Should_Return_Server_Error()
        {
            //Given
            _mockHttp.When(HttpMethod.Post, "*")
                    .Respond(_ =>
                    {
                        throw new Exception();
                    });

            // When
            var result = await _sut.CreatePayment(It.IsAny<CreatePaymentAcquirerRequest>());

            // Then
            result.IsServerError.ShouldBeTrue();
        }
    }
}