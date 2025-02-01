using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace Vrossi.ScaffoldCore.Application.Accounts.Commands
{
    public class PerformLogin : Request<Account>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public class Handler : DefaultRequestHandler<PerformLogin, Response<Account>>
        {
            private readonly IAccountRepository _accountRepository;

            public Handler(IApplicationEventPublisher applicationEventPublisher, IAccountRepository accountRepository) : base(applicationEventPublisher)
            {
                _accountRepository = accountRepository;
            }

            public override async Task<Response<Account>> Handle(PerformLogin request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindByEmail(request.Email);

                if (account == null)
                    return Response<Account>.Error("Email not registered");

                if (!BC.Verify(request.Password, account.Password))
                    return Response<Account>.Error("Invalid Login or Password");

                account.UpdateLastLogin();

                return Response<Account>.Success(account);
            }
        }
    }
}
