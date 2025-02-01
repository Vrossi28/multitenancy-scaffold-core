using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Accounts.Queries;
using Vrossi.ScaffoldCore.Core.Security;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication;
using Vrossi.ScaffoldCore.WebApi.Models.Accounts;
using System.Security.Principal;
using BC = BCrypt.Net.BCrypt;
using Vrossi.ScaffoldCore.Application.Tenants.Commands;

namespace Vrossi.ScaffoldCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status500InternalServerError)]
    public class AccountController : DefaultController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger, IMediator mediator) : base(mediator)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login", Name = "Login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostLogin([FromBody] PerformLoginModel model, [FromServices] ITokenProvider jwtAuthManager)
        {
            var response = await _mediator.Send(new PerformLogin()
            {
                Email = model.Email.ToLower(),
                Password = model.Password
            });

            if (!response.IsSuccess)
                return Unauthorized(response.Errors);

            var token = jwtAuthManager.GenerateToken(response.Data);

            return Ok(new LoginResponseDto()
            {
                Token = token,
                Account = new AccountDto()
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    LastName = response.Data.LastName,
                    Admin = response.Data.Admin,
                    Email = response.Data.Email,
                    DefaultId = response.Data?.Default.Id ?? Guid.Empty
                }
            });
        }

        [HttpPost("create", Name = "Create")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel model)
        {
            var accountResponse = await _mediator.Send(new CreateAccount()
            {
                Email = model.Email.ToLower(),
                Password = model.Password,
                LastName = model.LastName,
                Name = model.Name,
                TenantName = model.TenantName,
                IncludeTenant = model.IncludeTenant,
            });

            return FromResponse(accountResponse);
        }

        [Authorize]
        [HttpGet("", Name = "Logged User")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLoggedUser()
        {
            var response = await _mediator.Send(new GetLoggedAccount());

            if (response.IsNotFound)
                return NotFound();

            if (!response.IsSuccess)
                return Error(response.Errors);

            return Ok(new AccountDto()
            {
                Id = response.Data.Id,
                Name = response.Data.Name,
                LastName = response.Data.LastName,
                Admin = response.Data.Admin,
                Email = response.Data.Email,
                DefaultId = response.Data.DefaultId
            });
        }

        [HttpPost("change-default", Name = "ChangeDefault")]
        [Authorize(Policy = AppPolicyNames.Everyone)]
        [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PostChangeDefault([FromBody] ChangeDefaultModel model)
        {
            var response = await _mediator.Send(new ChangeDefault()
            {
                NewTenantId = model.TenantId
            });

            return FromResponse(response);
        }

        private bool HandleTenantDetails(bool includeTenant, string tenantName) => includeTenant && !String.IsNullOrEmpty(tenantName);
    }
}
