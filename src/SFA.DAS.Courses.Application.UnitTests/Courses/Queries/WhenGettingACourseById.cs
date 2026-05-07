using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetCourse;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingACourseById
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandard_From_Service_By_LarsCode(
            GetCourseByIdQuery query,
            Course courseFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCourseByIdQueryHandler handler)
        {
            // Arrange
            courseFromService.LarsCode = 1234.ToString();
            query.Id = courseFromService.LarsCode.ToString();
            mockStandardsService
                .Setup(service => service.GetCourseByAnyId(courseFromService.LarsCode))
                .ReturnsAsync(courseFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Course.Should().BeEquivalentTo(courseFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandard_From_Service_By_IFateReferenceNumber(
            GetCourseByIdQuery query,
            Course courseFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCourseByIdQueryHandler handler)
        {
            // Arrange
            query.Id = "ST0012";
            mockStandardsService
                .Setup(service => service.GetCourseByAnyId(query.Id))
                .ReturnsAsync(courseFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Course.Should().BeEquivalentTo(courseFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandard_From_Service_By_StandardUId(
            GetCourseByIdQuery query,
            Course courseFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCourseByIdQueryHandler handler)
        {
            // Arrange
            query.Id = "ST0012_1.2";
            mockStandardsService
                .Setup(service => service.GetCourseByAnyId(query.Id))
                .ReturnsAsync(courseFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Course.Should().BeEquivalentTo(courseFromService);
        }

        [Test]
        [MoqInlineAutoData(true)]
        [MoqInlineAutoData(false)]
        public async Task Then_Gets_ApprenticeshipStandard_With_CoronationEmblem_Set(
            bool coronationEmblem,
            GetCourseByIdQuery query,
            Course courseFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetCourseByIdQueryHandler handler)
        {
            // Arrange
            courseFromService.CoronationEmblem = coronationEmblem;
            mockStandardsService
                .Setup(service => service.GetCourseByAnyId(query.Id))
                .ReturnsAsync(courseFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Course.CoronationEmblem.Should().Be(coronationEmblem);
        }
    }
}
