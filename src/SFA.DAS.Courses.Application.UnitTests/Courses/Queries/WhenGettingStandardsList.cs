using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsList;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingStandardsList
    {
        //And_Fails_Validation - maybe in the future

        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Service(
            int count,
            GetStandardsListQuery query,
            List<Standard> standards,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsListQueryHandler handler)
        {
            mockStandardsService
                .Setup(service => service.GetStandardsList(
                    query.Keyword, 
                    query.RouteIds,
                    query.Levels))
                .ReturnsAsync(standards);
            mockStandardsService
                .Setup(service => service.Count())
                .ReturnsAsync(count);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Standards.Should().BeEquivalentTo(standards);
            result.Total.Should().Be(count);
            result.TotalFiltered.Should().Be(standards.Count);
        }

        [Test, MoqAutoData]
        public async Task And_Has_Keyword_And_No_Results_Then_Logs(
            int count,
            GetStandardsListQuery query,
            [Frozen] Mock<ILogger<GetStandardsListQueryHandler>> mockLogger,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsListQueryHandler handler)
        {
            var standards = new List<Standard>();
            mockStandardsService
                .Setup(service => service.GetStandardsList(
                    query.Keyword, 
                    query.RouteIds,
                    query.Levels))
                .ReturnsAsync(standards);
            mockStandardsService
                .Setup(service => service.Count())
                .ReturnsAsync(count);

            await handler.Handle(query, CancellationToken.None);

            mockLogger.Verify(logger => logger.Log(
                    LogLevel.Information, 
                    0, 
                    It.IsAny<It.IsAnyType>(),
                    null,
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()));
        }
    }
}
