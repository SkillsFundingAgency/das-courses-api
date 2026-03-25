using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.Customisations;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingActiveStandardsByIfateReferenceNumber : StandardRepositoryTestBase
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Apprenticeships(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            SetupContext(mockDataContext, data);

            // Act
            var standards = await repository.GetActiveStandardsByIfateReferenceNumbers(
                [data.ActiveValidApprenticeshipStandards[0].IfateReferenceNumber, data.ActiveInvalidApprenticeshipStandards[0].IfateReferenceNumber], CourseType.Apprenticeship);

            // Assert
            standards.Should().HaveCount(1);
            standards[0].IfateReferenceNumber.Should().Be(data.ActiveValidApprenticeshipStandards[0].IfateReferenceNumber);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_ShortCourses(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            SetupContext(mockDataContext, data);

            // Act
            var standards = await repository.GetActiveStandardsByIfateReferenceNumbers(
                [data.ActiveValidShortCourseStandards[0].IfateReferenceNumber, data.ActiveInvalidShortCourseStandards[0].IfateReferenceNumber], CourseType.ShortCourse);

            // Assert
            standards.Should().HaveCount(1);
            standards[0].IfateReferenceNumber.Should().Be(data.ActiveValidShortCourseStandards[0].IfateReferenceNumber);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Returns_Latest_Standards_For_Distinct_LarsCodes_Sharing_The_Same_IfateReferenceNumber(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var ifateReferenceNumber = "ST0001";

            var version1 = data.ActiveValidApprenticeshipStandards[0];
            version1.IfateReferenceNumber = ifateReferenceNumber;
            version1.LarsCode = "1001";
            version1.Version = "1.0";
            version1.VersionMajor = 1;
            version1.VersionMinor = 0;

            var version2 = data.ActiveValidApprenticeshipStandards[1];
            version2.IfateReferenceNumber = ifateReferenceNumber;
            version2.LarsCode = "1002";
            version2.Version = "2.0";
            version2.VersionMajor = 2;
            version2.VersionMinor = 0;

            SetupContext(mockDataContext, data);

            // Act
            var standards = await repository.GetActiveStandardsByIfateReferenceNumbers(
                new List<string> { ifateReferenceNumber },
                CourseType.Apprenticeship);

            // Assert
            standards.Should().HaveCount(2);
            standards.Select(x => x.LarsCode).Should().BeEquivalentTo("1001", "1002");
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Returns_Latest_ShortCourses_For_Distinct_LarsCodes_Sharing_The_Same_IfateReferenceNumber(
            [StandardRepositoryTestData] StandardRepositoryTestData data,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            // Arrange
            var ifateReferenceNumber = "AU0001";

            var version1 = data.ActiveValidShortCourseStandards[0];
            version1.IfateReferenceNumber = ifateReferenceNumber;
            version1.LarsCode = "ZSC01001";
            version1.Version = "1.0";
            version1.VersionMajor = 1;
            version1.VersionMinor = 0;

            var version2 = data.ActiveValidShortCourseStandards[1];
            version2.IfateReferenceNumber = ifateReferenceNumber;
            version2.LarsCode = "ZSC01002";
            version2.Version = "2.0";
            version2.VersionMajor = 2;
            version2.VersionMinor = 0;

            SetupContext(mockDataContext, data);

            // Act
            var standards = await repository.GetActiveStandardsByIfateReferenceNumbers(
                new List<string> { ifateReferenceNumber },
                CourseType.ShortCourse);

            // Assert
            standards.Should().HaveCount(2);
            standards.Select(x => x.LarsCode).Should().BeEquivalentTo("ZSC01001", "ZSC01002");
        }
    }
}
