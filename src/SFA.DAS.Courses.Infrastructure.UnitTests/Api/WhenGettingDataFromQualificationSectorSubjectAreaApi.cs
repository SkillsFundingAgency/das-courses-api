using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WhenGettingDataFromQualificationSectorSubjectAreaApi
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_And_An_Item_Array_Is_Returned(
            List<Domain.ImportTypes.QualificationItemList> qualificationItemList)
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(qualificationItemList)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri($"{Constants.QualificationSectorSubjectAreaUrl}entries/"));
            var client = new HttpClient(httpMessageHandler.Object);
            var apprenticeshipService = new QualificationSectorSubjectAreaService(client);
            
            //Act
            var actual = await apprenticeshipService.GetEntries();
            
            //Assert
            actual.Should().BeEquivalentTo(qualificationItemList);
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.Headers.Contains("Accept")
                        && c.Headers.GetValues("Accept").Single().Equals("application/json")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
        
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called_And_An_Item_Detail_Is_Returned(
            string itemHash,
            Domain.ImportTypes.QualificationItem qualificationItem)
        {
            //Arrange
            var response = new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(qualificationItem)),
                StatusCode = HttpStatusCode.Accepted
            };
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, new Uri($"{Constants.QualificationSectorSubjectAreaUrl}items/{itemHash}"));
            var client = new HttpClient(httpMessageHandler.Object);
            var apprenticeshipService = new QualificationSectorSubjectAreaService(client);
            
            //Act
            var actual = await apprenticeshipService.GetEntry(itemHash);
            
            //Assert
            actual.Should().BeEquivalentTo(qualificationItem);
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync",Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.Headers.Contains("Accept")
                        && c.Headers.GetValues("Accept").Single().Equals("application/json")
                    ),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}