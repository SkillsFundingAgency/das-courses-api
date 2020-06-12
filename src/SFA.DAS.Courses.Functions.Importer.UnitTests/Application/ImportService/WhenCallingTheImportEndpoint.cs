using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.Courses.Functions.Importer.Application.Services;
using SFA.DAS.Courses.Functions.Importer.Domain.Configuration;
using SFA.DAS.Courses.Functions.Importer.Domain.Interfaces;

namespace SFA.DAS.Courses.Functions.Importer.UnitTests.Application.ImportService
{
    public class WhenCallingTheImportEndpoint
    {
        [Test, AutoData]
        public void Then_The_Dataload_Endpoint_Is_Called(
            string authToken,
            ImporterConfiguration config)
        {
            //Arrange
            var azureClientCredentialHelper = new Mock<IAzureClientCredentialHelper>();
            azureClientCredentialHelper.Setup(x => x.GetAccessTokenAsync()).ReturnsAsync(authToken);
            var configuration = new Mock<IOptions<ImporterConfiguration>>();
            config.Url = "https://test.local/";
            configuration.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = SetupMessageHandlerMock(response, $"{config.Url}ops/dataload");
            var client = new HttpClient(httpMessageHandler.Object);
            var service = new ImportDataService(client, configuration.Object, azureClientCredentialHelper.Object, new ImporterEnvironment("TEST"));
            
            //Act
            service.Import();

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Equals($"{config.Url}ops/dataload")
                        && c.Headers.Authorization.Scheme.Equals("Bearer")
                        && c.Headers.Authorization.Parameter.Equals(authToken)),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Test, AutoData]
        public void Then_The_Bearer_Token_Is_Not_Added_If_Local(
            ImporterConfiguration config)
        {
            //Arrange
            var configuration = new Mock<IOptions<ImporterConfiguration>>();
            config.Url = "https://test.local/";
            configuration.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = SetupMessageHandlerMock(response, $"{config.Url}ops/dataload");
            var client = new HttpClient(httpMessageHandler.Object);
            var service = new ImportDataService(client, configuration.Object, Mock.Of<IAzureClientCredentialHelper>(), new ImporterEnvironment("LOCAL"));
            
            //Act
            service.Import();
            
            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Equals($"{config.Url}ops/dataload")
                        && c.Headers.Authorization == null),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
        
        private Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, string baseUrl)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(baseUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}