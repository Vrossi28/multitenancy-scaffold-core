using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Accounts.Commands
{
    public class ChangeDefault : Request
    {
        public Guid NewTenantId { get; init; }
        public class Handler : DefaultRequestHandler<ChangeDefault, Response>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly ITenantRepository _tenantRepository;
            public Handler(IApplicationEventPublisher applicationEventPublisher, IAccountRepository accountRepository, ITenantRepository tenantRepository) : base(applicationEventPublisher)
            {
                _accountRepository = accountRepository;
                _tenantRepository = tenantRepository;
            }
            public override async Task<Response> Handle(ChangeDefault request, CancellationToken cancellationToken)
            {
                var conta = await _accountRepository.GetByIdWithDefaultAndProfiles(request.UserId);

                if (conta == null)
                    return Response.NotFound();

                if (!conta.Admin && !conta.Profiles.Any(q => q.Tenant.Id == request.XTenantId))
                    return Response.Error("Account is not linked to the requested tenant");

                var tenant = await _tenantRepository.FindById(request.NewTenantId);

                conta.ChangeDefault(tenant);

                _accountRepository.Update(conta);

                return Response.Success();
            }
        }
    }
}
