using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Application.Tenants.Queries;
using Vrossi.ScaffoldCore.Core.Enums;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;
using Vrossi.ScaffoldCore.WebApi.Controllers;
using Vrossi.ScaffoldCore.WebApi.Models.Tenants;

namespace Vrossi.ScaffoldCore.UnitTest.WebApi.Controllers
{
    [TestClass]
    public class TenantControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private TenantController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new TenantController(_mockMediator.Object);
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var query = new RetrievePaginatedList { Page = 1, PageSize = 10 };
            var tenants = new TenantPaginatedListDto
            {
                Items = new List<TenantDto> { DtoFixtures.GetTenant() }
            };
            _mockMediator.Setup(m => m.Send(query, default))
                .ReturnsAsync(Response<TenantPaginatedListDto>.Success(tenants));

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(tenants, okResult.Value);
        }

        [TestMethod]
        public async Task Post_ShouldReturnOk_WhenTenantIsCreatedSuccessfully()
        {
            // Arrange
            var model = new CreateTenantModel { Name = "NewTenant" };
            var createdTenant = DtoFixtures.GetTenant(model.Name);
            var profile = DtoFixtures.GetAccount();

            _mockMediator.Setup(m => m.Send(It.Is<CreateTenant>(c => c.Name == model.Name), default))
                .ReturnsAsync(Response<TenantDto>.Success(createdTenant));

            _mockMediator.Setup(m => m.Send(It.Is<CreateProfile>(c => c.NewTenantId == createdTenant.Id && c.Profile == RoleType.Administrator), default)).ReturnsAsync(Response<AccountDto>.Success(profile));

            // Act
            var result = await _controller.Post(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(createdTenant, okResult.Value);
        }

        [TestMethod]
        public async Task Post_ShouldReturnBadRequest_WhenCreationFails()
        {
            // Arrange
            var model = new CreateTenantModel { Name = "NewTenant" };

            _mockMediator.Setup(m => m.Send(It.Is<CreateTenant>(c => c.Name == model.Name), default))
                .ReturnsAsync(Response<TenantDto>.Error("Error creating tenant"));

            // Act
            var result = await _controller.Post(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
