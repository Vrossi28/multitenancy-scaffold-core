using Microsoft.AspNetCore.Mvc;

namespace Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore
{
    public class CustomModelStateValidator
    {
        public static IActionResult ValidateModelState(ActionContext context)
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);

            var envelope = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = string.Join(";", errors)
            };

            return new BadRequestObjectResult(envelope);
        }
    }
}
