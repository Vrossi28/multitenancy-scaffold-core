﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.Messages
{
    public class DomainEvent : Event<Guid>, INotification
    {
    }
}
