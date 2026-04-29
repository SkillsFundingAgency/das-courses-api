using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetCoursesByIFateReference;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAllVersionsOfACourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_All_Course_Versions_From_Service(
            GetCoursesByIFateReferenceQuery query,
            List<Course> courses,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCoursesByIFateReferenceQueryHandler handler)
        {
            // Arrange
            mockStandardsService
                .Setup(service => service.GetAllVersionsOfACourse(query.IFateReferenceNumber))
                .ReturnsAsync(courses);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Courses.Should().BeEquivalentTo(courses);
        }
    }
}
