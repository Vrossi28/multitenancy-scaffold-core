using MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    internal class TransactionDbContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response
    {
        private readonly ScaffoldCoreContext _scaffoldCoreContext;

        public TransactionDbContextBehavior(ScaffoldCoreContext scaffoldCoreContext)
        {
            _scaffoldCoreContext = scaffoldCoreContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();
            try
            {
                await _scaffoldCoreContext.SaveChangesIfHasChanges(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                await _scaffoldCoreContext.Database.RollbackTransactionAsync(cancellationToken);
            }
            return response;
        }
    }
}
