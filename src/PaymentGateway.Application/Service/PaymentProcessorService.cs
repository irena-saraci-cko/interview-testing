using AutoMapper;

using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.Application.Dtos.GetPayment;
using PaymentGateway.BankAcquirer.Dtos;
using PaymentGateway.BankAcquirer.Services;
using PaymentGateway.Common.ServiceResponses;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

using Logger = Serilog.ILogger;

namespace PaymentGateway.Application.Service
{
    public class PaymentProcessorService : IPaymentProcessorService
    {
        private readonly IMapper _mapper;
        private readonly IAcquirerService _acquirerService;
        private readonly Logger _logger;
        private static List<Payment> _payments = new List<Payment>();
        public PaymentProcessorService(IMapper mapper, IAcquirerService acquirerService, Logger logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _acquirerService = acquirerService ?? throw new ArgumentNullException(nameof(acquirerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ServiceResponse<CreatePaymentApiResponse>> CreatePayment(CreatePaymentApiRequest paymentRequest)
        {
            //Send the request to the acquirer
            _logger.Information("Calling the acquirer bank");
            var acquirerResponse = await _acquirerService
                .CreatePayment(_mapper.Map<CreatePaymentAcquirerRequest>(paymentRequest));

            return acquirerResponse.Match(
                success => ProcessAcquirerResponse(success, paymentRequest),
                validationError => new ValidationError(validationError.PaymentStatus, validationError.ErrorCode, validationError.ErrorMessages),
                notFoundError => new NotFoundError(),
                serverError => new BadGatewayError(),
                BadGatewayError => new BadGatewayError(),
                timeoutError => new TimeoutError()
            );
        }

        public async Task<ServiceResponse<GetPaymentApiResponse>> GetPayment(Guid paymentId)
        {
            var payment = _payments.SingleOrDefault(p => p.Id.Equals(paymentId));
            if (payment is not null)
            {
                return _mapper.Map<GetPaymentApiResponse>(payment);
            }
            return new NotFoundError();
        }

        private ServiceResponse<CreatePaymentApiResponse> ProcessAcquirerResponse(CreatePaymentAcquirerResponse acquirerResponse, CreatePaymentApiRequest paymentRequest)
        {
            //save the payment
            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.Id = Guid.NewGuid();
            payment.Status = acquirerResponse!.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined;
            payment.AuthorizationCode = acquirerResponse.AuthorizationCode;
            _payments.Add(payment);

            return _mapper.Map<CreatePaymentApiResponse>(payment);
        }
    }
}