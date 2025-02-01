using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Application.Emails.Commands;
using Vrossi.ScaffoldCore.Application.Tenants.Models;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.WebApi.Controllers;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;
using Vrossi.ScaffoldCore.WebApi.Models.Emails;

namespace Vrossi.ScaffoldCore.UnitTest.WebApi.Controllers
{
    [TestClass]
    public class EmailControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private EmailController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new EmailController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task SendContactEmail_ReturnsOk_WhenMediatorReturnsSuccess()
        {
            // Arrange
            var model = new RequestContactModel
            {
                Email = "vrossi@test.com",
                Message = "Hello",
                Name = "Vinicius Rossi",
                Phone = "123456789"
            };

            var expectedResponse = Response.Success();
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendEmail>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.SendContactEmail(model);

            // Assert
            var noContentResult = result as NoContentResult;
            Assert.AreEqual(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [TestMethod]
        public async Task SendContactEmail_ReturnsError_WhenMediatorReturnsFailure()
        {
            // Arrange
            var model = new RequestContactModel
            {
                Email = "vrossi@test.com",
                Message = "Hello",
                Name = "Vinicius Rossi",
                Phone = "123456789"
            };

            var expectedResponse = Response.Error("Error occurred");
            _mediatorMock.Setup(m => m.Send(It.IsAny<SendEmail>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.SendContactEmail(model);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }
    }
}
