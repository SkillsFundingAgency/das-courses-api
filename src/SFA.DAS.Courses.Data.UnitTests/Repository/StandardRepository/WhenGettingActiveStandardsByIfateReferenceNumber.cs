using System.Collections.Generic;
using System.Linq;
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

        [Test, RecursiveMoqAutoData]
        public async Task Then_Returns_Latest_Standards_For_Distinct_LarsCodes_Sharing_The_Same_IfateReferenceNumber(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var ifateReferenceNumber = "ST0001";

            var version1 = activeValidApprenticeshipStandards[0];
            version1.IfateReferenceNumber = ifateReferenceNumber;
            version1.LarsCode = "1001";
            version1.Version = "1.0";
            version1.VersionMajor = 1;
            version1.VersionMinor = 0;

            var version2 = activeValidApprenticeshipStandards[1];
            version2.IfateReferenceNumber = ifateReferenceNumber;
            version2.LarsCode = "1002";
            version2.Version = "2.0";
            version2.VersionMajor = 2;
            version2.VersionMinor = 0;

            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(new List<Standard> { version1, version2 });

            mockDataContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var standards = await repository.GetActiveStandardsByIfateReferenceNumbers(
                new List<string> { ifateReferenceNumber },
                CourseType.Apprenticeship);

            // Assert
            standards.Should().HaveCount(2);
            standards.Select(x => x.LarsCode).Should().BeEquivalentTo("1001", "1002");
        }
    }
}
