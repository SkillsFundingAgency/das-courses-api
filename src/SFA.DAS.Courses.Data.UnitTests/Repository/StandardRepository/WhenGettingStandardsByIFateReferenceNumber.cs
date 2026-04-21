using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingStandardsByIFateReferenceNumber : StandardRepositoryTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Versions_Of_That_Standard_Are_Returned(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var iFateReferenceNumber = "ST0099";
            var active = data.ActiveValidApprenticeshipStandards[0];
            active.IfateReferenceNumber = iFateReferenceNumber;
            active.Version = "1.1";
            active.StandardUId = $"{iFateReferenceNumber}_1.1";

            var retired = data.RetiredApprenticeshipStandards[0];
            retired.IfateReferenceNumber = iFateReferenceNumber;
            retired.Version = "1.0";
            retired.StandardUId = $"{iFateReferenceNumber}_1.0";

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(iFateReferenceNumber, CourseType.Apprenticeship);

            // Assert
            Assert.That(actualStandards, Is.Not.Null);
            actualStandards.Should().BeEquivalentTo(new List<Standard> { active, retired }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_All_Versions_Of_That_ShortCourse_Are_Returned(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var iFateReferenceNumber = "AU0099";
            var active = data.ActiveValidShortCourseStandards[0];
            active.IfateReferenceNumber = iFateReferenceNumber;
            active.Version = "1.1";
            active.StandardUId = $"{iFateReferenceNumber}_1.1";

            var retired = data.RetiredShortCourseStandards[0];
            retired.IfateReferenceNumber = iFateReferenceNumber;
            retired.Version = "1.0";
            active.StandardUId = $"{iFateReferenceNumber}_1.0";

            SetupContext(mockDataContext, data);

            // Act
            var actualStandards = await repository.GetStandards(iFateReferenceNumber, CourseType.ShortCourse);

            // Assert
            Assert.That(actualStandards, Is.Not.Null);
            actualStandards.Should().BeEquivalentTo(new List<Standard> { active, retired }, EquivalencyAssertionOptionsHelper.DoNotIncludeAllPropertiesExcludes());
        }
    }
}
