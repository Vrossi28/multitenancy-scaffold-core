using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Messages
{
    public abstract class Event<T> : Message<T>
    {
        protected Event() => Timestamp = DateTime.Now;
        public DateTime Timestamp { get; set; }
    }
}
