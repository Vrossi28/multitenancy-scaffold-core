using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Tenants.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Common.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Tenants.Commands
{
    [TestClass]
    public class CreateTenantTests
    {
        private Mock<ITenantRepository> _mockTenantRepository;
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<IMapper> _mockMapper;
        private Mock<IApplicationEventPublisher> _mockApplicationEventPublisher;

        [TestInitialize]
        public void SetUp()
        {
            _mockTenantRepository = new Mock<ITenantRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockApplicationEventPublisher = new Mock<IApplicationEventPublisher>();
        }
         
        [TestMethod]
        public async Task Handle_ShouldReturnError_WhenAccountNotFound()
        {
            // Arrange
            var request = new CreateTenant { UserId = Guid.NewGuid(), Name = "Test Tenant" };

            _mockAccountRepository.Setup(repo => repo.FindById(request.UserId))
                .ReturnsAsync((Account)null);

            var handler = new CreateTenant.Handler(
                _mockApplicationEventPublisher.Object,
                _mockTenantRepository.Object,
                _mockAccountRepository.Object,
                _mockMapper.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Account not found", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ShouldCreateTenant_WhenAccountExists()
        {
            // Arrange
            var request = new CreateTenant { UserId = Guid.NewGuid(), Name = "Test Tenant" };
            var account = DomainFixtures.GetAccount();
            var tenant = DomainFixtures.GetTenant(request.Name);
            var tenantDto = new TenantDto { Id = tenant.Id, Name = request.Name };

            _mockAccountRepository.Setup(repo => repo.FindById(request.UserId))
                .ReturnsAsync(account);

            _mockTenantRepository.Setup(repo => repo.AddAsync(It.IsAny<Tenant>()))
                .Returns(Task.CompletedTask);

            var handler = new CreateTenant.Handler(
                _mockApplicationEventPublisher.Object,
                _mockTenantRepository.Object,
                _mockAccountRepository.Object,
                _mockMapper.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _mockTenantRepository.Verify(repo => repo.AddAsync(It.IsAny<Tenant>()), Times.Once);
        }
    }
}
