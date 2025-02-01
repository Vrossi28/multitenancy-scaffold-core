using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Messages
{
    public class ApplicationEvent : Event<Guid>, INotification
    {
        public ApplicationEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
        public Guid? TenantId { get; internal set; }
    }
}
