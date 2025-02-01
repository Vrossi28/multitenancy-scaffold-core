using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vrossi.ScaffoldCore.Application.Emails.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Queries;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using Vrossi.ScaffoldCore.WebApi.Models.Emails;

namespace Vrossi.ScaffoldCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = AppPolicyNames.Everyone)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
    public class EmailController : DefaultController
    {
        public EmailController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("/request-contact", Name = "SendContactEmail")]
        [Authorize(Policy = AppPolicyNames.AdminsOrDirectors)]
        [ProducesResponseType(typeof(TenantPaginatedListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendContactEmail([FromBody] RequestContactModel model)
        {
            var response = await _mediator.Send(new SendEmail()
            {
                Email = model.Email,
                Message = model.Message,
                Name = model.Name,
                Phone = model.Phone
            });
            return FromResponse(response);
        }
    }
}
