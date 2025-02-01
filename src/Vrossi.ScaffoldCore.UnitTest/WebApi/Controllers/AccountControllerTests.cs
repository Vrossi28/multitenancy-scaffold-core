using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Accounts.Commands;
using Vrossi.ScaffoldCore.Application.Accounts.Models;
using Vrossi.ScaffoldCore.Application.Accounts.Queries;
using Vrossi.ScaffoldCore.Application.Tenants.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Infrastructure.Domain;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.UnitTest.Fixtures;
using Vrossi.ScaffoldCore.WebApi.Controllers;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.Authentication;
using Vrossi.ScaffoldCore.WebApi.Models.Accounts;

namespace Vrossi.ScaffoldCore.UnitTest.WebApi.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<ILogger<AccountController>> _mockLogger;
        private Mock<IMediator> _mockMediator;
        private Mock<ITokenProvider> _mockJwtAuthManager;
        private Mock<JwtSecurityTokenHandler> _mockJwtSecurityTokenHandler;
        private AccountController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockJwtSecurityTokenHandler = new Mock<JwtSecurityTokenHandler>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _mockMediator = new Mock<IMediator>();
            _mockJwtAuthManager = new Mock<ITokenProvider>();
            _controller = new AccountController(_mockConfiguration.Object, _mockLogger.Object, _mockMediator.Object);
        }

        [TestMethod]
        public async Task PostLogin_ShouldReturnUnauthorized_WhenLoginFails()
        {
            // Arrange
            var model = new PerformLoginModel { Email = "vrossi@test.com", Password = "wrongpassword" };
            _mockMediator.Setup(m => m.Send(It.IsAny<PerformLogin>(), default))
                .ReturnsAsync(Response<Account>.Error("Invalid credentials"));

            // Act
            var result = await _controller.PostLogin(model, _mockJwtAuthManager.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [TestMethod]
        public async Task PostLogin_ShouldReturnOk_WhenLoginSucceeds()
        {
            // Arrange
            var model = new PerformLoginModel { Email = "vrossi@test.com", Password = "password" };
            var accountData = DtoFixtures.GetAccount(email: model.Email, admin: true);
            var account = new Account(accountData.Name, accountData.LastName, model.Email, model.Password);
            account.ChangeDefault(new Mock<Tenant>().Object);
            var accountResponse = Response<Account>.Success(account);

            _mockMediator.Setup(m => m.Send(It.IsAny<PerformLogin>(), default)).ReturnsAsync(accountResponse);
            _mockJwtAuthManager.Setup(j => j.GenerateToken(It.IsAny<Account>())).Returns("token");

            // Act
            var result = await _controller.PostLogin(model, _mockJwtAuthManager.Object);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var response = okResult.Value as LoginResponseDto;
            Assert.AreEqual("token", response.Token);
        }


        [TestMethod]
        public async Task CreateAccount_ShouldReturnOk_WhenAccountCreationSucceeds()
        {
            // Arrange
            var model = new CreateAccountModel { Email = "vrossi@test.com", Password = "password", Name = "Vinicius", LastName = "Rossi" };
            var tenantData = DtoFixtures.GetTenant();
            var accountData = DtoFixtures.GetAccount(model.Name, model.LastName, model.Email, tenantData.Id.ToString());
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateAccount>(), default))
                .ReturnsAsync(Response<AccountDto>.Success(accountData));

            // Act
            var result = await _controller.CreateAccount(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var response = okResult.Value as AccountDto;
            Assert.AreEqual(accountData.Id, response.Id);
        }

        [TestMethod]
        public async Task CreateAccount_IncludingTenant_ShouldReturnOk_WhenAccountCreationSucceeds()
        {
            // Arrange
            var model = new CreateAccountModel { Email = "vrossi@test.com", Password = "password", Name = "Vinicius", LastName = "Rossi", IncludeTenant = true, TenantName = "tenant" };
            var tenantData = DtoFixtures.GetTenant(model.TenantName);
            var accountData = DtoFixtures.GetAccount(model.Name,  model.LastName, model.Email, tenantData.Id.ToString());

            _mockMediator.Setup(m => m.Send(It.IsAny<CreateAccount>(), default))
                .ReturnsAsync(Response<AccountDto>.Success(accountData));

            // Act
            var result = await _controller.CreateAccount(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var response = okResult.Value as AccountDto;
            Assert.AreEqual(accountData.Id, response.Id);
            Assert.AreEqual(accountData.DefaultId, tenantData.Id);
        }

        [TestMethod]
        public async Task GetLoggedUser_ShouldReturnNotFound_WhenUserNotFound()
        {
            // Arrange
            _mockMediator.Setup(m => m.Send(It.IsAny<GetLoggedAccount>(), default))
                .ReturnsAsync(Response<AccountDto>.NotFound());

            // Act
            var result = await _controller.GetLoggedUser();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task PostChangeDefault_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var model = new ChangeDefaultModel { TenantId = Guid.NewGuid() };
            _mockMediator.Setup(m => m.Send(It.IsAny<ChangeDefault>(), default))
                .ReturnsAsync(Response.Success());

            // Act
            var result = await _controller.PostChangeDefault(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}
