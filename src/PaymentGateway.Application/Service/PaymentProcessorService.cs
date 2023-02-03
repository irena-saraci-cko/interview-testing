using AutoMapper;

using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.BankAcquirer.Dtos;
using PaymentGateway.BankAcquirer.Services;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Application.Service
{
    public class PaymentProcessorService : IPaymentProcessorService
    {
        private readonly IMapper _mapper;
        private readonly IAcquirerService _acquirerService;
        private List<Payment> _payments = new List<Payment>();
        public PaymentProcessorService(IMapper mapper, IAcquirerService acquirerService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _acquirerService = acquirerService ?? throw new ArgumentNullException(nameof(acquirerService));
        }
        public async Task<CreatePaymentResponseDto> CreatePayment(CreatePaymentRequestDto paymentRequest)
        {
            //Send the request to the acquirer
            var acquirerResponse = await _acquirerService
                .CreatePayment(_mapper.Map<CreatePaymentAcquirerRequest>(paymentRequest));
            
            //save the payment
            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.Id = Guid.NewGuid();
            payment.Status = acquirerResponse.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined;
            payment.AuthorizationCode = acquirerResponse.AuthorizationCode;
            _payments.Add(payment);

            return _mapper.Map<CreatePaymentResponseDto>(payment);
        }
    }
}