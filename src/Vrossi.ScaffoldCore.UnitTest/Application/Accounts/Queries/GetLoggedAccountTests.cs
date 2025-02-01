using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Accounts.Queries;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Accounts.Queries
{
    [TestClass]
    public class GetLoggedAccountTests
    {
        private Mock<IAccountRepository> _mockAccountRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void SetUp()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Handle_ShouldReturnNotFound_WhenAccountIsNotFound()
        {
            // Arrange
            var request = new GetLoggedAccount { UserId = Guid.NewGuid() };

            _mockAccountRepository.Setup(repo => repo.FindById(It.IsAny<Guid>()))
                                  .ReturnsAsync((Account)null);

            var handler = new GetLoggedAccount.Handler(_mockAccountRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Resource not found", result.ErrorMessage);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnAccountDto_WhenAccountIsFound()
        {
            // Arrange
            var account = DomainFixtures.GetAccount();

            var request = new GetLoggedAccount { UserId = Guid.NewGuid() };
            var accountDto = DtoFixtures.GetAccount();

            _mockAccountRepository.Setup(repo => repo.FindById(It.IsAny<Guid>()))
                                  .ReturnsAsync(account);

            var handler = new GetLoggedAccount.Handler(_mockAccountRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
