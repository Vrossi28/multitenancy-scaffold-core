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
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Tenants.Commands
{
    public class CreateTenant : TenantRequest
    {
        public class Handler : DefaultRequestHandler<CreateTenant, Response<TenantDto>>
        {
            private readonly ITenantRepository _tenantRepository;
            private readonly IAccountRepository _accountRepository;
            private readonly IMapper _mapper;

            public Handler(IApplicationEventPublisher applicationEventPublisher, ITenantRepository tenantRepository,
                IAccountRepository accountRepository,
                IMapper mapper) : base(applicationEventPublisher)
            {
                _tenantRepository = tenantRepository;
                _accountRepository = accountRepository;
                _mapper = mapper;
            }
            public override async Task<Response<TenantDto>> Handle(CreateTenant request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindById(request.UserId);
                if (account is null)
                    return Response<TenantDto>.EntityNotFound("Account not found");

                Tenant tenant = new(request.Name);
                await _tenantRepository.AddAsync(tenant);
                //TODO Limitation: Required to force new jwt creation to include new tenant

                return _mapper.MapToResponse<TenantDto>(tenant);
            }
        }
    }
}
