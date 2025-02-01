using AutoMapper;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Accounts.Commands
{
    public class CreateProfile : Request<AccountDto>
    {
        public Guid NewTenantId { get; set; }
        public RoleType Profile {  get; set; }

        public class Handler : DefaultRequestHandler<CreateProfile, Response<AccountDto>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly ITenantRepository _tenantRepository;
            private readonly IAccountProfileRepository _accountProfileRepository;
            private readonly IMapper _mapper;

            public Handler(IApplicationEventPublisher applicationEventPublisher, IAccountRepository accountRepository, ITenantRepository tenantRepository, IAccountProfileRepository accountProfileRepository, IMapper mapper) : base(applicationEventPublisher)
            {
                _accountRepository = accountRepository;
                _tenantRepository = tenantRepository;
                _accountProfileRepository = accountProfileRepository;
                _mapper = mapper;
            }

            public override async Task<Response<AccountDto>> Handle(CreateProfile request, CancellationToken cancellationToken)
            {
                var tenant = await _tenantRepository.FindById(request.NewTenantId);
                if (tenant is null)
                    return Response<AccountDto>.Error("Tenant not Found");

                var account = await _accountRepository.FindById(request.UserId);
                if (account is null)
                    return Response<AccountDto>.EntityNotFound("Account not found");

                var accountProfile = new AccountProfile(request.Profile, request.NewTenantId, request.UserId);
                await _accountProfileRepository.AddAsync(accountProfile);

                return _mapper.MapToResponse<AccountDto>(account);
            }
        }
    }
}
