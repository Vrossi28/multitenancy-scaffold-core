using AutoMapper;
using MediatR;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Application.Tenants.Queries
{
    public class RetrievePaginatedList : PaginationRequest<TenantPaginatedListDto>
    {
        public class Handler : IRequestHandler<RetrievePaginatedList, Response<TenantPaginatedListDto>>
        {
            private readonly ITenantRepository _tenantRepository;
            private readonly IMapper _mapper;

            public Handler(ITenantRepository tenantRepository, IMapper mapper)
            {
                _tenantRepository = tenantRepository;
                _mapper = mapper;
            }

            public async Task<Response<TenantPaginatedListDto>> Handle(RetrievePaginatedList request, CancellationToken cancellationToken)
            {
                var result = await _tenantRepository.List(request);
                return Response<TenantPaginatedListDto>.Success(_mapper.Map<TenantPaginatedListDto>(result));
            }
        }
    }
}
