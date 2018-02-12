using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using Xunit;

namespace ProjectToBuild.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexTest()
        {
            var sut = new Controllers.HomeController();
            var result = sut.Index() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void AboutTest()
        {
            var expectedMessage = "Your application description page.";
            var sut = new Controllers.HomeController();
            var result = sut.About() as ViewResult;

            Assert.NotNull(result);

            var actualMessage = result.ViewData["Message"];

            Assert.Equal(actualMessage, expectedMessage);

        }

        [Fact]
        public void ContactTest()
        {
            var expectedMessage = "Your contact page.";
            var sut = new Controllers.HomeController();
            var result = sut.Contact() as ViewResult;

            Assert.NotNull(result);

            var actualMessage = result.ViewData["Message"];

            Assert.Equal(actualMessage, expectedMessage);
        }
        
        [Fact]
        public void ErrorTest()
        {
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(_ => _.TraceIdentifier).Returns(Guid.NewGuid().ToString());
            var cc = new ControllerContext(
                new ActionContext(
                    httpContext.Object,
                    new RouteData(),
                    new ControllerActionDescriptor()));

            var sut = new Controllers.HomeController();
            sut.ControllerContext = cc;

            var result = sut.Error();

        }

    }
}
