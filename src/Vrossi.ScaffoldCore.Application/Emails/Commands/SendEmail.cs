using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Email;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpFunctionalExtensions.Result;

namespace Vrossi.ScaffoldCore.Application.Emails.Commands
{
    public class SendEmail : Request
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }

        public class Handler : DefaultRequestHandler<SendEmail, Response>
        {
            private readonly IHangfireJobScheduler _hangfireJobScheduler;
            private readonly IOptions<EmailIdentity> _emailIdentity;

            public Handler(IApplicationEventPublisher applicationEventPublisher, IHangfireJobScheduler hangfireJobScheduler, IOptions<EmailIdentity> emailIdentity) : base(applicationEventPublisher)
            {
                _hangfireJobScheduler = hangfireJobScheduler;
                _emailIdentity = emailIdentity;
            }
            public override async Task<Response> Handle(SendEmail request, CancellationToken cancellationToken)
            {
                var welcomeRecipients = new List<string>()
                {
                    request.Email
                };

                await _hangfireJobScheduler.Enqueue(new EmailModelRequest()
                {
                    Recipients = welcomeRecipients,
                    ModelName = EmailModel.Welcome
                });

                var contactRecipients = _emailIdentity.Value.ContactRecipients.ToList();

                var attributes = new Dictionary<string, string>()
                {
                    { EmailConstants.Email, request.Email },
                    { EmailConstants.Name, request.Name },
                    { EmailConstants.Message, request.Message },
                    { EmailConstants.Phone, request.Phone },
                };

                await _hangfireJobScheduler.Enqueue(new EmailModelRequest()
                {
                    Recipients = contactRecipients,
                    ModelName = EmailModel.ContactRequested,
                    Attributes = attributes
                });

                return Response.Success();
            }
        }
    }
}
