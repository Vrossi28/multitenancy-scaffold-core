using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;
using BC = BCrypt.Net.BCrypt;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Accounts.Commands
{
    [TestClass]
    public class PerformLoginTests
    {
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<IApplicationEventPublisher> _mockApplicationEventPublisher;

        [TestInitialize]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockApplicationEventPublisher = new Mock<IApplicationEventPublisher>();
        }

        [TestMethod]
        public async Task Handle_ShouldReturnError_WhenEmailNotRegistered()
        {
            // Arrange
            var request = new PerformLogin
            {
                Email = "nonexistent@example.com",
                Password = "somepassword"
            };

            _mockAccountRepository.Setup(repo => repo.FindByEmail(It.IsAny<string>()))
                                  .ReturnsAsync((Account)null);

            var handler = new PerformLogin.Handler(_mockApplicationEventPublisher.Object, _mockAccountRepository.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Email not registered", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnError_WhenPasswordIsInvalid()
        {
            // Arrange
            var account = DomainFixtures.GetAccount(email: "vinicius@test.com", password: BC.HashPassword("correctpassword"));

            var request = new PerformLogin
            {
                Email = "vinicius@test.com",
                Password = "wrongpassword"
            };

            _mockAccountRepository.Setup(repo => repo.FindByEmail(It.IsAny<string>()))
                                  .ReturnsAsync(account);

            var handler = new PerformLogin.Handler(_mockApplicationEventPublisher.Object, _mockAccountRepository.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Invalid Login or Password", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnSuccess_WhenLoginIsValid()
        {
            // Arrange
            var account = DomainFixtures.GetAccount(email: "vinicius@test.com", password: "correctpassword");

            var request = new PerformLogin
            {
                Email = "vinicius@test.com",
                Password = "correctpassword"
            };

            _mockAccountRepository.Setup(repo => repo.FindByEmail(It.IsAny<string>()))
                                  .ReturnsAsync(account);

            var handler = new PerformLogin.Handler(_mockApplicationEventPublisher.Object, _mockAccountRepository.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(account, result.Data);
            _mockAccountRepository.Verify(repo => repo.FindByEmail(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldUpdateLastLogin_WhenLoginIsSuccessful()
        {
            // Arrange
            var account = DomainFixtures.GetAccount(password: "correctpassword");

            var request = new PerformLogin
            {
                Email = "vinicius@test.com",
                Password = "correctpassword"
            };

            _mockAccountRepository.Setup(repo => repo.FindByEmail(It.IsAny<string>()))
                                  .ReturnsAsync(account);

            var handler = new PerformLogin.Handler(_mockApplicationEventPublisher.Object, _mockAccountRepository.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _mockAccountRepository.Verify(repo => repo.FindByEmail(It.IsAny<string>()), Times.Once);
            Assert.IsTrue(account.LastLoginDate > DateTime.MinValue);
        }
    }
}
