using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingActiveStandardsByIfateReferenceNumber
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Standards(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [FoundationApprenticeshipStandardsLarsValid] List<Standard> activeValidFoundationApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> shortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidFoundationApprenticeshipStandards);
            allStandards.AddRange(shortCourseStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDataContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var standards = await repository.GetActiveStandardsByIfateReferenceNumbers(
                [activeValidApprenticeshipStandards[0].IfateReferenceNumber, activeInvalidApprenticeshipStandards[0].IfateReferenceNumber], CourseType.Apprenticeship);

            // Assert
            standards.Should().HaveCount(1);
            standards[0].IfateReferenceNumber.Should().Be(activeValidApprenticeshipStandards[0].IfateReferenceNumber);
        }
    }
}
