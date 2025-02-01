using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Accounts.Commands
{
    [TestClass]
    public class CreateAccountTests
    {
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<ITenantRepository> _tenantRepositoryMock;
        private Mock<IApplicationEventPublisher> _applicationEventPublisherMock;
        private Mock<IMapper> _mapperMock;
        private CreateAccount.Handler _handler;

        [TestInitialize]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _tenantRepositoryMock = new Mock<ITenantRepository>();
            _applicationEventPublisherMock = new Mock<IApplicationEventPublisher>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateAccount.Handler(
                _applicationEventPublisherMock.Object,
                _accountRepositoryMock.Object,
                _tenantRepositoryMock.Object,
                _mapperMock.Object);
        }

        [TestMethod]
        public async Task Handle_CreatesAccountWithoutTenant_WhenIncludeTenantIsFalse()
        {
            // Arrange
            var request = new CreateAccount
            {
                Name = "Vinicius",
                LastName = "Rossi",
                Email = "vincius@text.com",
                Password = "password123",
                IncludeTenant = false
            };

            var account = new Account(request.Name, request.LastName, request.Email, request.Password);
            var accountDto = new AccountDto();

            _accountRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _accountRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Account>()), Times.Once);
            _tenantRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Tenant>()), Times.Never);
        }

        [TestMethod]
        public async Task Handle_CreatesAccountWithTenant_WhenIncludeTenantIsTrue()
        {
            // Arrange
            var request = new CreateAccount
            {
                Name = "Vinicius",
                LastName = "Rossi",
                Email = "vincius@text.com",
                Password = "password123",
                IncludeTenant = true,
                TenantName = "TestTenant"
            };

            var tenant = new Tenant(request.TenantName);
            var account = new Account(request.Name, request.LastName, request.Email, request.Password, true, tenant);
            var accountDto = new AccountDto();

            _tenantRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Tenant>()))
                .Returns(Task.CompletedTask);
            _accountRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Account>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            _tenantRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Tenant>()), Times.Once);
            _accountRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Account>()), Times.Once);
        }
    }
}
