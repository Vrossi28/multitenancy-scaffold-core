using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Accounts.Commands
{
    [TestClass]
    public class CreateProfileTests
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<ITenantRepository> _tenantRepositoryMock;
        private Mock<IAccountProfileRepository> _accountProfileRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IApplicationEventPublisher> _applicationEventPublisherMock;
        private CreateProfile.Handler _handler;

        [TestInitialize]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _tenantRepositoryMock = new Mock<ITenantRepository>();
            _accountProfileRepositoryMock = new Mock<IAccountProfileRepository>();
            _mapperMock = new Mock<IMapper>();
            _applicationEventPublisherMock = new Mock<IApplicationEventPublisher>();

            _handler = new CreateProfile.Handler(
                _applicationEventPublisherMock.Object,
                _accountRepositoryMock.Object,
                _tenantRepositoryMock.Object,
                _accountProfileRepositoryMock.Object,
                _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_ReturnsError_WhenTenantNotFound()
        {
            // Arrange
            var request = new CreateProfile { NewTenantId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            _tenantRepositoryMock.Setup(repo => repo.FindById(request.NewTenantId))
                .ReturnsAsync((Tenant)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Tenant not Found", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ReturnsEntityNotFound_WhenAccountNotFound()
        {
            // Arrange
            var request = new CreateProfile { NewTenantId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            var tenant = DomainFixtures.GetTenant();

            _tenantRepositoryMock.Setup(repo => repo.FindById(request.NewTenantId))
                .ReturnsAsync(tenant);
            _accountRepositoryMock.Setup(repo => repo.FindById(request.UserId))
                .ReturnsAsync((Account)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsNotFound);
            Assert.AreEqual("Account not found", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ReturnsSuccess_WhenProfileIsCreated()
        {
            // Arrange
            var request = new CreateProfile { NewTenantId = Guid.NewGuid(), UserId = Guid.NewGuid(), Profile = RoleType.Administrator };
            var tenant = DomainFixtures.GetTenant();
            var account = DomainFixtures.GetAccount();
            var accountProfile = new AccountProfile(request.Profile, request.NewTenantId, request.UserId);

            _tenantRepositoryMock.Setup(repo => repo.FindById(request.NewTenantId))
                .ReturnsAsync(tenant);
            _accountRepositoryMock.Setup(repo => repo.FindById(request.UserId))
                .ReturnsAsync(account);
            _accountProfileRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<AccountProfile>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _accountProfileRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<AccountProfile>()), Times.Once);
        }
    }
}
