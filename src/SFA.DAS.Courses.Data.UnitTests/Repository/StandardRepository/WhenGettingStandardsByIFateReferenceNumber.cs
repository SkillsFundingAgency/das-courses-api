using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingStandardsByIFateReferenceNumber
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Versions_Of_That_Standard_Are_Returned(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> activeValidShortCoursesStandards,
            [Frozen] Mock<ICoursesDataContext> mockDbContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var iFateReferenceNumber = "ST001";
            var active = activeValidApprenticeshipStandards[0];
            active.IfateReferenceNumber = iFateReferenceNumber;
            active.Version = "1.1";
            active.StandardUId = "ST001_1.1";

            var retired = retiredApprenticeshipStandards[0];
            retired.IfateReferenceNumber = iFateReferenceNumber;
            retired.Version = "1.0";
            retired.StandardUId = "ST001_1.0";


            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(activeValidShortCoursesStandards);
            mockDbContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            mockDbContext
                .Setup(c => c.ApprenticeshipFunding)
                .ReturnsDbSet(new List<ApprenticeshipFunding>());

            // Act
            var actualStandards = await repository.GetStandards(iFateReferenceNumber, CourseType.Apprenticeship);

            // Assert
            Assert.That(actualStandards, Is.Not.Null);
            actualStandards.Should().BeEquivalentTo(new List<Standard> { active, retired }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }
    }
}
