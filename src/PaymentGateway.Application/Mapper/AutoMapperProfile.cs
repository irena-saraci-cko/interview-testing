using AutoMapper;

using PaymentGateway.Application.Dtos.CreatePayment;
using PaymentGateway.Application.Dtos.GetPayment;
using PaymentGateway.Application.Extensions;
using PaymentGateway.BankAcquirer.Dtos;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Payment, CreatePaymentApiResponse>()
                .ForMember(c => c.MaskedCardNumber, y => y.MapFrom((s) => s.CardNumber.ToMaskNumber()))
                .ForMember(c => c.Status, y => y.MapFrom((s, d) => s.Status.ToString()));

            CreateMap<Payment, GetPaymentApiResponse>()
                .ForMember(c => c.MaskedCardNumber, y => y.MapFrom((s) => s.CardNumber.ToMaskNumber()))
                .ForMember(c => c.Status, y => y.MapFrom((s, d) => s.Status.ToString()));

            CreateMap<CreatePaymentApiRequest, Payment>();

            CreateMap<CreatePaymentApiRequest, CreatePaymentAcquirerRequest>()
            .ForMember(x => x.ExpiryDate, y => y.MapFrom(s => s.ExpiryDate()));
        }
    }
}