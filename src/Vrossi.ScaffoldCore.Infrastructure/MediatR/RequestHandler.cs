using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    public abstract class RequestHandler<TUnitOfWork, TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response
    {
        protected readonly TUnitOfWork _unitOfWork;
        protected readonly IApplicationEventPublisher _applicationEventPublisher;

        public RequestHandler(TUnitOfWork unitOfWork, IApplicationEventPublisher applicationEventPublisher)
        {
            _unitOfWork = unitOfWork;
            _applicationEventPublisher = applicationEventPublisher;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public abstract class DefaultRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Response
    {
        protected readonly IApplicationEventPublisher _applicationEventPublisher;

        public DefaultRequestHandler(IApplicationEventPublisher applicationEventPublisher)
        {
            _applicationEventPublisher = applicationEventPublisher;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
