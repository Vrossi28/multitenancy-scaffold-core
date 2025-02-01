using Vrossi.ScaffoldCore.Core.Domain;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Domain
{
    public class Tenant : AggregateRoot
    {
        protected Tenant() { }
        public Tenant(string nome) : base(Guid.NewGuid())
        {
            SetName(nome);
        }
        public string Name { get; private set; }
        public Tenant SetName(string name)
        {
            AssertionConcern.AssertArgumentNotNull(name, "Name is mandatory");
            AssertionConcern.AssertArgumentLength(name, 255, "Name exceeds the maximum length");
            Name = name.Trim();
            return this;
        }
    }
}
