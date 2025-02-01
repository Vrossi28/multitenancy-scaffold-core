using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using System.Net.Mime;

namespace Vrossi.ScaffoldCore.WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public abstract class DefaultController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected DefaultController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult FromResponse(Response response)
        {
            if (response == null || response.IsNotFound)
                return NotFound(response.ErrorMessage);

            if (!response.IsSuccess)
                return Error(response.Errors);

            return NoContent();
        }
        public IActionResult FromResponse<TDto>(Response<TDto> response)
          where TDto : class
        {
            if (response == null || response.IsNotFound)
                return NotFound();

            if (!response.IsSuccess)
                return Error(response.Errors);

            return Ok(response.Data);
        }
        public new IActionResult Ok(object value)
        {
            if (value == null)
                return NotFound();

            return base.Ok(value);
        }
        protected IActionResult NotFound(string error = null)
        {
            return NotFound(new ErrorDetails()
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = error ?? "Resource not found"
            });
        }
        protected IActionResult Error(string error)
        {
            return BadRequest(new ErrorDetails()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = error
            });
        }
        protected IActionResult Error(IEnumerable<string> errors)
        {
            return BadRequest(new ErrorDetails()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = string.Join(";", errors)
            });
        }
        protected IActionResult Unauthorized(IEnumerable<string> errors)
        {
            return Unauthorized(new ErrorDetails()
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = string.Join(";", errors)
            });
        }
    }
}
