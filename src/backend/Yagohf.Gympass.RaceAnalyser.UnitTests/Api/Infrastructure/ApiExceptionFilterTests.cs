using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Filters;
using Yagohf.Gympass.RaceAnalyser.Infrastructure.Exception;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Api.Infrastructure
{
    [TestClass]
    public class ApiExceptionFilterTests
    {
        private readonly Mock<ILogger<ApiExceptionFilter>> _loggerMock;
        private ApiExceptionFilter _apiExceptionFilter;

        public ApiExceptionFilterTests()
        {
            this._loggerMock = new Mock<ILogger<ApiExceptionFilter>>();
        }

        [TestInitialize]
        public void Inicializar()
        {
            this._apiExceptionFilter = new ApiExceptionFilter(this._loggerMock.Object);
        }

        [TestMethod]
        public void Testar_OnException_BusinessException()
        {
            //Arrange.
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
                {
                    ActionName = "Get",
                    ControllerName = "InfrastructureTests",
                    MethodInfo = typeof(InfrastructureTestsController).GetMethods().Single(x => x.Name == "Get")
                },
            };

            ExceptionContext context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            context.Exception = new BusinessException("Exceção de negócio");

            //Act.
            this._apiExceptionFilter.OnException(context);

            //Assert.
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(400, ((JsonResult)context.Result).StatusCode);
        }

        [TestMethod]
        public void Testar_OnException_Exception()
        {
            //Arrange.
            var actionContext = new ActionContext
            {
                HttpContext = new DefaultHttpContext(),
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
                {
                    ActionName = "Get",
                    ControllerName = "InfrastructureTests",
                    MethodInfo = typeof(InfrastructureTestsController).GetMethods().Single(x => x.Name == "Get")
                },
            };

            ExceptionContext context = new ExceptionContext(actionContext, new List<IFilterMetadata>());
            context.Exception = new Exception("Exceção comum");

            //Act.
            this._apiExceptionFilter.OnException(context);

            //Assert.
            Assert.IsNotNull(context.Result);
            Assert.AreEqual(500, ((JsonResult)context.Result).StatusCode);
        }
    }
}
