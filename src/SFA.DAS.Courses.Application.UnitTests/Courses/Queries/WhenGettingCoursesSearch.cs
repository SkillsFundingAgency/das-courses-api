using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetCoursesSearch;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingCoursesSearch
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Courses_From_Service(
            int count,
            GetCoursesSearchQuery query,
            List<Course> standards,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCoursesSearchQueryHandler handler)
        {
            // Arrange
            mockStandardsService
                .Setup(service => service.GetCoursesList(
                    query.Keyword,
                    query.RouteIds,
                    query.Levels,
                    query.OrderBy,
                    query.Filter,
                    query.IncludeAllProperties,
                    query.LearningType,
                    query.CourseType))
                .ReturnsAsync(standards);
            
            mockStandardsService
                .Setup(service => service.Count(query.Filter, query.CourseType))
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
            GetCoursesSearchQuery query,
            [Frozen] Mock<ILogger<GetCoursesSearchQueryHandler>> mockLogger,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCoursesSearchQueryHandler handler)
        {
            // Arrange
            var standards = new List<Course>();
            query.Levels = new List<int>();
            query.RouteIds = new List<int>();
            query.LearningType = null;
            mockStandardsService
                .Setup(service => service.GetCoursesList(
                    query.Keyword,
                    It.Is<List<int>>(c => c.Count == 0),
                    It.Is<List<int>>(c => c.Count == 0),
                    query.OrderBy,
                    query.Filter,
                    query.IncludeAllProperties,
                    query.LearningType,
                    query.CourseType))
                .ReturnsAsync(standards);
            mockStandardsService
                .Setup(service => service.Count(query.Filter, null))
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
