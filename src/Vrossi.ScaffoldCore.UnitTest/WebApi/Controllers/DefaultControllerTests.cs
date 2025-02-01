using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vrossi.ScaffoldCore.Infrastructure.MediatR;
using Vrossi.ScaffoldCore.WebApi.Controllers;
using Vrossi.ScaffoldCore.WebApi.Infrastructure.AspNetCore;

namespace Vrossi.ScaffoldCore.UnitTest.WebApi.Controllers
{
    [TestClass]
    public class DefaultControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private DefaultController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TestController(_mediatorMock.Object);
        }

        [TestMethod]
        public void FromResponse_ResponseIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var response = Response.NotFound();

            // Act
            var result = _controller.FromResponse(response);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual("Resource not found", ((ErrorDetails)notFoundResult.Value).Message);
        }

        [TestMethod]
        public void FromResponse_ResponseIsSuccess_ReturnsNoContent()
        {
            // Arrange
            var response = Response.Success();

            // Act
            var result = _controller.FromResponse(response);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void FromResponse_ResponseIsNotSuccess_ReturnsBadRequest()
        {
            // Arrange
            var response = Response.Error("Some error occurred");

            // Act
            var result = _controller.FromResponse(response);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Some error occurred", ((ErrorDetails)badRequestResult.Value).Message);
        }

        [TestMethod]
        public void FromResponseWithDto_ResponseIsSuccess_ReturnsOkWithData()
        {
            // Arrange
            var response = Response<string>.Success("Success");

            // Act
            var result = _controller.FromResponse(response);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.AreEqual("Success", okResult.Value);
        }

        [TestMethod]
        public void FromResponseWithDto_ResponseIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var response = Response.NotFound();

            // Act
            var result = _controller.FromResponse(response);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void FromResponseWithDto_ResponseIsNotSuccess_ReturnsBadRequest()
        {
            // Arrange
            var response = Response.Error("Error 1");
            response.AddErrors((new List<string> { "Error 2", "Error 3" }));

            // Act
            var result = _controller.FromResponse(response);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Error 1;Error 2;Error 3", ((ErrorDetails)badRequestResult.Value).Message);
        }

        // Custom controller to test abstract DefaultController
        public class TestController : DefaultController
        {
            public TestController(IMediator mediator) : base(mediator) { }
        }
    }
}
