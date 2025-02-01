using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Messages
{
    public abstract class Message<T>
    {
        protected Message() => MessageType = GetType().Name;
        public string MessageType { get; protected set; }
        public T AggregateId { get; set; }
    }
}
