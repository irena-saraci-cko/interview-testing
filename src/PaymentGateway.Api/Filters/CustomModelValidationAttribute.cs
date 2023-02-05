using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentGateway.Common.ServiceResponses;

namespace PaymentGateway.Api.Filters
{
    public class CustomModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Any())
                        .SelectMany(v => v.Errors)
                        .Select(v => v.ErrorMessage)
                        .ToList();

                var responseObj = new ValidationError("Rejected", "invalid_data", errors);

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
        }
    }
}