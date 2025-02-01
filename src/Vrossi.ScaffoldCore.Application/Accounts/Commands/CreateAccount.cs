using AutoMapper;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Vrossi.ScaffoldCore.Application.Accounts.Commands
{
    public class CreateAccount : Request<AccountDto>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IncludeTenant { get; set; } = false;
        public string TenantName { get; set; } = string.Empty;

        public class Handler : DefaultRequestHandler<CreateAccount, Response<AccountDto>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly ITenantRepository _tenantRepository;
            private readonly IMapper _mapper;

            public Handler(IApplicationEventPublisher applicationEventPublisher, IAccountRepository accountRepository, ITenantRepository tenantRepository, IMapper mapper) : base(applicationEventPublisher)
            {
                _accountRepository = accountRepository;
                _tenantRepository = tenantRepository;
                _mapper = mapper;
            }

            public override async Task<Response<AccountDto>> Handle(CreateAccount request, CancellationToken cancellationToken)
            {
                Account account;

                if (request.IncludeTenant && !String.IsNullOrEmpty(request.TenantName))
                {
                    var tenant = new Tenant(request.TenantName);
                    await _tenantRepository.AddAsync(tenant);

                    account = new Account(request.Name, request.LastName, request.Email, request.Password, true, tenant);
                }
                else
                {
                    account = new Account(request.Name, request.LastName, request.Email, request.Password);
                }

                await _accountRepository.AddAsync(account);

                return _mapper.MapToResponse<AccountDto>(account);
            }
        }
    }
}
