using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Persistence.Data
{
    public abstract class Entity : Entity<Guid>
    {
        protected Entity() { }
        protected Entity(Guid id)
        {
            Id = id;
            CreatedAt = UpdatedAt = DateTime.UtcNow;
            IsNew = true;
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsNew { get; }
    }
}
