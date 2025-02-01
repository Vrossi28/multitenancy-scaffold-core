using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Queries;
using Vrossi.ScaffoldCore.Common.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.Persistence.Data;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;

namespace Vrossi.ScaffoldCore.UnitTest.Application.Tenants.Queries
{
    [TestClass]
    public class RetrievePaginatedListTests
    {
        private Mock<ITenantRepository> _mockTenantRepository;
        private Mock<IMapper> _mockMapper;

        [TestInitialize]
        public void SetUp()
        {
            _mockTenantRepository = new Mock<ITenantRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        public async Task Handle_ShouldReturnMappedPaginatedList_WhenRepositoryReturnsData()
        {
            // Arrange
            var request = new RetrievePaginatedList { Page = 1, PageSize = 10 };
            var tenantDtoList = new TenantPaginatedListDto();
            var tenantList = new PaginationResult<Tenant>() { Items = new List<Tenant> { DomainFixtures.GetTenant()} };

            _mockTenantRepository
                .Setup(repo => repo.List(request))
                .ReturnsAsync(tenantList);

            _mockMapper
                .Setup(mapper => mapper.Map<TenantPaginatedListDto>(tenantList))
                .Returns(tenantDtoList);

            var handler = new RetrievePaginatedList.Handler(
                _mockTenantRepository.Object,
                _mockMapper.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);

            _mockTenantRepository.Verify(repo => repo.List(request), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<TenantPaginatedListDto>(It.IsAny<PaginationResult<Tenant>>()), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnEmptyPaginatedList_WhenRepositoryReturnsEmptyData()
        {
            // Arrange
            var request = new RetrievePaginatedList { Page = 1, PageSize = 10 };
            var emptyTenantList = new PaginationResult<Tenant>();
            var emptyTenantDtoList = new TenantPaginatedListDto();

            _mockTenantRepository
                .Setup(repo => repo.List(request))
                .ReturnsAsync(emptyTenantList);

            _mockMapper
                .Setup(mapper => mapper.Map<TenantPaginatedListDto>(emptyTenantList))
                .Returns(emptyTenantDtoList);

            var handler = new RetrievePaginatedList.Handler(
                _mockTenantRepository.Object,
                _mockMapper.Object
            );

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);

            _mockTenantRepository.Verify(repo => repo.List(request), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<TenantPaginatedListDto>(It.IsAny<PaginationResult<Tenant>>()), Times.Once);
        }
    }
}
