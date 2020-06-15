using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Courses.Domain.Configuration;
using SFA.DAS.Courses.Infrastructure.Api;

namespace SFA.DAS.Courses.Infrastructure.UnitTests.Api
{
    public class WhenGettingDataFromInstituteOfApprenticeshipsApi
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_And_Standards_Returned(List<Domain.ImportTypes.Standard> importStandards)
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(importStandards)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = SetupMessageHandlerMock(response);
            var client = new HttpClient(httpMessageHandler.Object);
            var apprenticeshipService = new InstituteOfApprenticeshipService(client);
            
            //Act
            var standards = await apprenticeshipService.GetStandards();
            
            //Assert
            standards.Should().BeEquivalentTo(importStandards);
        }
        
        [Test]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown()
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(""),
                StatusCode = HttpStatusCode.BadRequest
            };
            var httpMessageHandler = SetupMessageHandlerMock(response);
            var client = new HttpClient(httpMessageHandler.Object);
            var apprenticeshipService = new InstituteOfApprenticeshipService(client);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apprenticeshipService.GetStandards());
            
        }
        
        private Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.RequestUri.AbsoluteUri.Equals(Constants.InstituteOfApprenticeshipsStandardsUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}