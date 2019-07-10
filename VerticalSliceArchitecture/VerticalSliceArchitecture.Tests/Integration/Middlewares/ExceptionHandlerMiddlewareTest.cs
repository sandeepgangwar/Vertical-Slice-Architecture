using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using VerticalSliceArchitecture.API.Middlewares;
using VerticalSliceArchitecture.Infrastructure.Exceptions;
using VerticalSliceArchitecture.Tests.Integration.Utilities;
using Xunit;

namespace RepositoryPattern.Tests.Integration.Middlewares
{
    
    public class ExceptionHandlerMiddlewareTest :IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        
        public ExceptionHandlerMiddlewareTest (TestServerFixture fixture)
        {
            _fixture = fixture;           
        }


        [Fact]
        public async Task WhenResourceNotFoundExceptionIsRaised_ExceptionHandlerMiddlewareShouldHandleItToCustomErrorResponseAndNotFoundHttpStatus()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware((innerHttpContext) =>
            {
                throw new ResourceNotFoundException("Test", "Test");
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
           
            
            //Act
            await middleware.Invoke(context,_fixture.CurrentEnvironment);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var response = JsonConvert.DeserializeObject<ErrorResponse>(streamText);

            //Assert
            response.ShouldNotBe(null);
            response.ShouldBeOfType<ErrorResponse>();
            response.Code.ShouldBe("Test");
            response.Message.ShouldBe("Test");
            context.Response.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task WhenInfrastrucureExceptionIsRaised_ExceptionHandlerMiddlewareShouldHandleItToCustomErrorResponseAndBadRequestHttpStatus()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware((innerHttpContext) =>
            {
                throw new InfrastructureException("Test", "Test");
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();


            //Act
            await middleware.Invoke(context,_fixture.CurrentEnvironment);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();
            var response = JsonConvert.DeserializeObject<ErrorResponse>(streamText);

            //Assert
            response.ShouldNotBe(null);
            response.ShouldBeOfType<ErrorResponse>();
            response.Code.ShouldBe("Test");
            response.Message.ShouldBe("Test");
            context.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task WhenUnauthorizedExceptionIsRaised_ExceptionHandlerMiddlewareShouldHandleItToUnauthorizedHttpStatus()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware((innerHttpContext) =>
            {
                throw new UnauthorizedAccessException();
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context,_fixture.CurrentEnvironment);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            
            //Assert
            context.Response.StatusCode.ShouldBe((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WhenAnUnExpectedExceptionIsRaised_CustomExceptionMiddlewareShouldHandleItToInternalServerErrorHttpStatus()
        {
            // Arrange
            var middleware = new ExceptionHandlerMiddleware(next: (innerHttpContext) =>
            {
                throw new Exception();
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context,_fixture.CurrentEnvironment);

            //Assert
            context.Response.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);


        }


    }

}
