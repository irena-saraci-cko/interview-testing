using OneOf;

namespace PaymentGateway.Common.ServiceResponses
{
    public class ServiceResponse<T>
        : OneOfBase<T, ValidationError, NotFoundError, ServerError, BadGatewayError, TimeoutError>
    {
        protected ServiceResponse(OneOf<T, ValidationError, NotFoundError, ServerError, BadGatewayError, TimeoutError> input) : base(input)
        {
        }
        public static implicit operator ServiceResponse<T>(T val) =>
            new ServiceResponse<T>(val);
        public static implicit operator ServiceResponse<T>(ValidationError validationError) =>
            new ServiceResponse<T>(validationError);
        public static implicit operator ServiceResponse<T>(NotFoundError notFoundError) =>
            new ServiceResponse<T>(notFoundError);
        public static implicit operator ServiceResponse<T>(ServerError serverError) =>
            new ServiceResponse<T>(serverError);
        public static implicit operator ServiceResponse<T>(BadGatewayError badGateway) =>
            new ServiceResponse<T>(badGateway);
        public static implicit operator ServiceResponse<T>(TimeoutError timeoutError) =>
             new ServiceResponse<T>(timeoutError);

        public bool IsSuccess => this.IsT0;
    }
}