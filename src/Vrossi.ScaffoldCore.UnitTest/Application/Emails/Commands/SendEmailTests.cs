using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Emails.Commands;
using Vrossi.ScaffoldCore.Common.Infrastructure.Email;
using Vrossi.ScaffoldCore.Common.Infrastructure.Hangfire;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Email;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Emails.Commands
{
    [TestClass]
    public class SendEmailTests
    {
        private Mock<IHangfireJobScheduler> _mockHangfireJobScheduler;
        private Mock<IOptions<EmailIdentity>> _mockEmailIdentity;
        private Mock<IApplicationEventPublisher> _mockApplicationEventPublisher;

        [TestInitialize]
        public void SetUp()
        {
            _mockHangfireJobScheduler = new Mock<IHangfireJobScheduler>();
            _mockEmailIdentity = new Mock<IOptions<EmailIdentity>>();
            _mockApplicationEventPublisher = new Mock<IApplicationEventPublisher>();

            // Mock email identity options
            var emailIdentity = new EmailIdentity
            {
                ContactRecipients = new[] { "contact@test.com" }
            };

            _mockEmailIdentity.Setup(e => e.Value).Returns(emailIdentity);
        }

        [TestMethod]
        public async Task Handle_ShouldEnqueueWelcomeEmail_AndContactEmail()
        {
            // Arrange
            var request = new SendEmail
            {
                Email = "vinicius@test.com",
                Name = "Vinicius Rossi",
                Message = "This is a test message",
                Phone = "123456789"
            };

            var handler = new SendEmail.Handler(
                _mockApplicationEventPublisher.Object,
                _mockHangfireJobScheduler.Object,
                _mockEmailIdentity.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);

            // Verify that the welcome email is scheduled
            _mockHangfireJobScheduler.Verify(scheduler => scheduler.Enqueue(It.Is<EmailModelRequest>(
                e => e.Recipients.Contains(request.Email) && e.ModelName == EmailModel.Welcome)), Times.Once);

            // Verify that the contact request email is scheduled
            _mockHangfireJobScheduler.Verify(scheduler => scheduler.Enqueue(It.Is<EmailModelRequest>(
                e => e.Recipients.Contains("contact@test.com") &&
                     e.ModelName == EmailModel.ContactRequested &&
                     e.Attributes[EmailConstants.Email] == request.Email &&
                     e.Attributes[EmailConstants.Name] == request.Name &&
                     e.Attributes[EmailConstants.Message] == request.Message &&
                     e.Attributes[EmailConstants.Phone] == request.Phone
            )), Times.Once);
        }
    }
}
