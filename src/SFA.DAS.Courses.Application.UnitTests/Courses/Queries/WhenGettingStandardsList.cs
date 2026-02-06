using System;
using System.Collections.Generic;
using System.Linq;
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

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

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
            // Arrange
            mockStandardsService
                .Setup(service => service.GetStandardsList(
                    query.Keyword,
                    query.RouteIds,
                    query.Levels,
                    query.OrderBy,
                    query.Filter,
                    query.IncludeAllProperties,
                    query.ApprenticeshipType,
                    CourseType.Apprenticeship))
                .ReturnsAsync(standards);
            
            mockStandardsService
                .Setup(service => service.Count(query.Filter, CourseType.Apprenticeship))
                .ReturnsAsync(count);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standards.Should().BeEquivalentTo(standards);
            result.Total.Should().Be(count);
            result.TotalFiltered.Should().Be(standards.Count);
        }

        [Test, MoqAutoData]
        public async Task And_Has_Keyword_And_No_Other_Filters_And_No_Results_Then_Logs(
            int count,
            GetStandardsListQuery query,
            [Frozen] Mock<ILogger<GetStandardsListQueryHandler>> mockLogger,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsListQueryHandler handler)
        {
            // Arrange
            var standards = new List<Standard>();
            query.Levels = new List<int>();
            query.RouteIds = new List<int>();
            query.ApprenticeshipType = null;
            mockStandardsService
                .Setup(service => service.GetStandardsList(
                    query.Keyword,
                    It.Is<List<int>>(c => c.Count == 0),
                    It.Is<List<int>>(c => c.Count == 0),
                    query.OrderBy,
                    query.Filter,
                    query.IncludeAllProperties,
                    null,
                    CourseType.Apprenticeship))
                .ReturnsAsync(standards);
            mockStandardsService
                .Setup(service => service.Count(query.Filter, CourseType.Apprenticeship))
                .ReturnsAsync(count);
            
            // Act
            await handler.Handle(query, CancellationToken.None);

            // Assert
            mockLogger.Verify(logger => logger.Log(
                    LogLevel.Information,
                    0,
                    It.IsAny<It.IsAnyType>(),
                    null,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }
    }
}
