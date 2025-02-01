using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Queries;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using Vrossi.ScaffoldCore.WebApi.Models.Tenants;

namespace Vrossi.ScaffoldCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = AppPolicyNames.Everyone)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
    public class TenantController : DefaultController
    {
        public TenantController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("", Name = "GetAllTenants")]
        [Authorize(Policy = AppPolicyNames.AdminsOrDirectors)]
        [ProducesResponseType(typeof(TenantPaginatedListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] RetrievePaginatedList query)
        {
            var response = await _mediator.Send(query);
            return FromResponse(response);
        }

        [HttpPost("", Name = "CreateTenant")]
        [Authorize(Policy = AppPolicyNames.AdminsOrDirectors)]
        [ProducesResponseType(typeof(TenantDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CreateTenantModel model)
        {
            var response = await _mediator.Send(new CreateTenant()
            {
                Name = model.Name,
            });

            if (response.IsSuccess)
            {
                await _mediator.Send(new CreateProfile()
                {
                    NewTenantId = response.Data.Id,
                    Profile = RoleType.Administrator
                });
            }

            return FromResponse(response);
        }
    }
}
