using AutoMapper;
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

namespace Vrossi.ScaffoldCore.UnitTest.Application.Accounts.Commands
{
    [TestClass]
    public class ChangeDefaultTests
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<ITenantRepository> _tenantRepositoryMock;
        private Mock<IApplicationEventPublisher> _applicationEventPublisherMock;
        private ChangeDefault.Handler _handler;

        [TestInitialize]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _tenantRepositoryMock = new Mock<ITenantRepository>();
            _applicationEventPublisherMock = new Mock<IApplicationEventPublisher>();

            _handler = new ChangeDefault.Handler(
                _applicationEventPublisherMock.Object,
                _accountRepositoryMock.Object,
                _tenantRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            var request = new ChangeDefault { UserId = Guid.NewGuid(), NewTenantId = Guid.NewGuid() };
            _accountRepositoryMock.Setup(repo => repo.GetByIdWithDefaultAndProfiles(request.UserId))
                .ReturnsAsync((Account)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsNotFound);
        }

        [TestMethod]
        public async Task Handle_ReturnsError_WhenAccountNotLinkedToTenant()
        {
            // Arrange
            var request = new ChangeDefault { UserId = Guid.NewGuid(), NewTenantId = Guid.NewGuid(), XTenantId = Guid.NewGuid() };
            var account = DomainFixtures.GetAccount();

            _accountRepositoryMock.Setup(repo => repo.GetByIdWithDefaultAndProfiles(request.UserId))
                .ReturnsAsync(account);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Account is not linked to the requested tenant", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ReturnsSuccess_WhenDefaultTenantIsChanged()
        {
            // Arrange
            var request = new ChangeDefault { UserId = Guid.NewGuid(), NewTenantId = Guid.NewGuid(), XTenantId = Guid.NewGuid() };
            var tenant = DomainFixtures.GetTenant();
            var account = DomainFixtures.GetAccount(tenant: tenant, admin: true);

            _accountRepositoryMock.Setup(repo => repo.GetByIdWithDefaultAndProfiles(request.UserId))
                .ReturnsAsync(account);
            _tenantRepositoryMock.Setup(repo => repo.FindById(It.IsAny<Guid>()))
                .ReturnsAsync(tenant);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _accountRepositoryMock.Verify(repo => repo.Update(account), Times.Once);
        }
    }
}
