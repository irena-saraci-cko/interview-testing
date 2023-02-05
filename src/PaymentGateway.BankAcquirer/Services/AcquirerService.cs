using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using PaymentGateway.BankAcquirer.Dtos;
using PaymentGateway.Common.ServiceResponses;

using Logger = Serilog.ILogger;

namespace PaymentGateway.BankAcquirer.Services
{
    public class AcquirerService : IAcquirerService
    {
        private readonly HttpClient _httpClient;
        private readonly Logger _logger;
        public AcquirerService(HttpClient httpClient, Logger logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ServiceResponse<CreatePaymentAcquirerResponse>> CreatePayment(CreatePaymentAcquirerRequest paymentRequest)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(paymentRequest, GetSettings()), Encoding.UTF8, MediaTypeNames.Application.Json),
            };
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            httpRequestMessage.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("UTF-8"));

            try
            {
                _logger.Information("Sending request to the acquirer bank");
                var response = await _httpClient.SendAsync(httpRequestMessage);
                if (response.StatusCode is HttpStatusCode.OK)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrWhiteSpace(responseJson))
                    {
                        var createPaymentResponse = JsonConvert.DeserializeObject<CreatePaymentAcquirerResponse>(responseJson);

                        if (createPaymentResponse is not null)
                            return createPaymentResponse;
                    }

                    _logger.Error("Invalid response");
                    return new ServerError();
                }
                else
                {
                    _logger.Error("The response status code is {HttpResponse}", response.StatusCode);
                    return new ValidationError("Rejected", "acquirer_invalid_request");
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.Error("HttpRequest exception happened when sending the request to the acquirerBank {Exception}", httpRequestException.Message);
                return new ServerError();
            }
            catch (TimeoutException timeoutException)
            {
                _logger.Error("Timeout exception happened when sending the request to the acquirerBank {Exception}", timeoutException.Message);
                return new TimeoutError();
            }
            catch (Exception exception)
            {
                _logger.Error("Exception happened when sending the request to the acquirerBank {Exception}", exception.Message);
                return new ServerError();
            }
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(),

                }
            };
            settings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

            settings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy()));
            return settings;
        }
    }
}