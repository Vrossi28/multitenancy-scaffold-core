using AutoMapper;
using MediatR;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Accounts.Queries
{
    public class GetLoggedAccount : Request<AccountDto>
    {
        public class Handler : IRequestHandler<GetLoggedAccount, Response<AccountDto>>
        {
            private readonly IAccountRepository _accountRepository;
            private readonly IMapper _mapper;

            public Handler(IAccountRepository accountRepository, IMapper mapper)
            {
                _accountRepository = accountRepository;
                _mapper = mapper;
            }

            public async Task<Response<AccountDto>> Handle(GetLoggedAccount request, CancellationToken cancellationToken)
            {
                var account = await _accountRepository.FindById(request.UserId);

                if (account is null)
                    return Response<AccountDto>.NotFound();

                return _mapper.MapToResponse<AccountDto>(account);
            }
        }
    }
}
