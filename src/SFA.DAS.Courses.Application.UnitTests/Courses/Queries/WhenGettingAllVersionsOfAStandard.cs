using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Application.Courses.Queries.GetStandardsByIFateReference;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

using CourseType = SFA.DAS.Courses.Domain.Entities.CourseType;

namespace SFA.DAS.Courses.Application.UnitTests.Courses.Queries
{
    public class WhenGettingAllVersionsOfAStandard
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipStandards_From_Service(
            GetStandardsByIFateReferenceQuery query,
            List<Standard> standards,
            [Frozen] Mock<IStandardsService> mockStandardsService,
            GetStandardsByIFateReferenceQueryHandler handler)
        {
            // Arrange
            mockStandardsService
                .Setup(service => service.GetAllVersionsOfAStandard(query.IFateReferenceNumber, CourseType.Apprenticeship))
                .ReturnsAsync(standards);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Standards.Should().BeEquivalentTo(standards);
        }
    }
}
