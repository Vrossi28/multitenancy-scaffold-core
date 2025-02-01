using Vrossi.ScaffoldCore.Infrastructure.Messages;
using Vrossi.ScaffoldCore.Infrastructure.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    internal class ApplicationEventPublisher : IApplicationEventPublisher
    {
        private readonly List<ApplicationEvent> _events = new();
        private readonly ITenantStrategy _tenantStrategy;
        public ApplicationEventPublisher(ITenantStrategy tenantStrategy)
        {
            _tenantStrategy = tenantStrategy;
        }
        public IReadOnlyList<ApplicationEvent> GetEvents()
        {
            var eventsInMemory = new List<ApplicationEvent>(_events);
            _events.Clear();
            return eventsInMemory;
        }
        public void Add<TEvent>(TEvent @event) where TEvent : ApplicationEvent
        {
            if (@event == null) return;

            @event.TenantId = _tenantStrategy.GetTenantId();

            _events.Add(@event);
        }
        public bool HasEvents => _events.Count > 0;
    }
    public interface IApplicationEventPublisher
    {
        void Add<TEvent>(TEvent @event) where TEvent : ApplicationEvent;
        IReadOnlyList<ApplicationEvent> GetEvents();
        bool HasEvents { get; }
    }
}
