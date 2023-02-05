using AutoMapper;

using PaymentGateway.Application.Dtos.CreatePayment;
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
        private List<Payment> _payments = new List<Payment>();
        public PaymentProcessorService(IMapper mapper, IAcquirerService acquirerService, Logger logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _acquirerService = acquirerService ?? throw new ArgumentNullException(nameof(acquirerService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ServiceResponse<CreatePaymentResponseDto>> CreatePayment(CreatePaymentRequestDto paymentRequest)
        {
            //Send the request to the acquirer
            _logger.Information("Calling the acquirer bank");
            var acquirerResponse = await _acquirerService
                .CreatePayment(_mapper.Map<CreatePaymentAcquirerRequest>(paymentRequest));

            return acquirerResponse.Match(
                success => ProcessAcquirerResponse(success, paymentRequest),
                validationError => new ValidationError(validationError.PaymentStatus, validationError.ErrorCode, validationError.ErrorMessages),
                serverError => new BadGatewayError(),
                BadGatewayError => new BadGatewayError(),
                timeoutError => new TimeoutError()
            );
        }

        private ServiceResponse<CreatePaymentResponseDto> ProcessAcquirerResponse(CreatePaymentAcquirerResponse acquirerResponse, CreatePaymentRequestDto paymentRequest)
        {
            //save the payment
            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.Id = Guid.NewGuid();
            payment.Status = acquirerResponse!.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined;
            payment.AuthorizationCode = acquirerResponse.AuthorizationCode;
            _payments.Add(payment);

            return _mapper.Map<CreatePaymentResponseDto>(payment);
        }
    }
}