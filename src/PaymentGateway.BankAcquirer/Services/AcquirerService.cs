using System.Net;
using System.Net.Mime;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using PaymentGateway.BankAcquirer.Dtos;
using PaymentGateway.Common.ServiceResponses;

namespace PaymentGateway.BankAcquirer.Services
{
    public class AcquirerService : IAcquirerService
    {
        private readonly HttpClient _httpClient;
        public AcquirerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ServiceResponse<CreatePaymentAcquirerResponse>> CreatePayment(CreatePaymentAcquirerRequest paymentRequest)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(paymentRequest, GetSettings()),
                    Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            try
            {
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
                }

                return new ServerError();
            }
            catch (HttpRequestException httpRequestException)
            {
                return new ServerError();
            }
            catch (TimeoutException timeoutException)
            {
                return new TimeoutError();
            }
            catch (Exception ex)
            {
                return new ServerError();
            }
        }

        private static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            settings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy()));
            return settings;
        }
    }
}