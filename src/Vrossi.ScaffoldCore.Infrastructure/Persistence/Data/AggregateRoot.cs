using Vrossi.ScaffoldCore.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Data
{
    public abstract class AggregateRoot : Entity
    {
        private List<DomainEvent> _domainEvents = new();
        protected AggregateRoot() { }
        protected AggregateRoot(Guid id) : base(id) { }
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;
        protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearEvents() => _domainEvents.Clear();
    }
}
