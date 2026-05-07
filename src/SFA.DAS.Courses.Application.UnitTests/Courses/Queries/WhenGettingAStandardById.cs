using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetCourse;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandard;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAStandardById
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandard_From_Service_By_LarsCode(
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            // Arrange
            standardFromService.LarsCode = 1234.ToString();
            query.Id = standardFromService.LarsCode.ToString();
            mockStandardsService
                .Setup(service => service.GetStandardByAnyId(standardFromService.LarsCode))
                .ReturnsAsync(standardFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standard.Should().BeEquivalentTo(standardFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandard_From_Service_By_IFateReferenceNumber(
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            // Arrange
            query.Id = "ST0012";
            mockStandardsService
                .Setup(service => service.GetStandardByAnyId(query.Id))
                .ReturnsAsync(standardFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standard.Should().BeEquivalentTo(standardFromService);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandard_From_Service_By_StandardUId(
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            // Arrange
            query.Id = "ST0012_1.2";
            mockStandardsService
                .Setup(service => service.GetStandardByAnyId(query.Id))
                .ReturnsAsync(standardFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standard.Should().BeEquivalentTo(standardFromService);
        }

        [Test]
        [MoqInlineAutoData(true)]
        [MoqInlineAutoData(false)]
        public async Task Then_Gets_ApprenticeshipStandard_With_CoronationEmblem_Set(
            bool coronationEmblem,
            GetStandardByIdQuery query,
            Standard standardFromService,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardByIdQueryHandler handler)
        {
            // Arrange
            standardFromService.CoronationEmblem = coronationEmblem;
            mockStandardsService
                .Setup(service => service.GetStandardByAnyId(query.Id))
                .ReturnsAsync(standardFromService);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standard.CoronationEmblem.Should().Be(coronationEmblem);
        }
    }
}
