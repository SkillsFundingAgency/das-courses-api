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
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingCount
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Standards(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
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
            allStandards.AddRange(shortCourseStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);
            
            // Act
            var count = await repository.Count(StandardFilter.ActiveAvailable, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(activeValidApprenticeshipStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Active_Standards(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
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
            allStandards.AddRange(shortCourseStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            // Act
            var count = await repository.Count(StandardFilter.Active, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(activeValidApprenticeshipStandards.Count + activeInvalidApprenticeshipStandards.Count + retiredApprenticeshipStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Active_Standards_Including_Retired_With_Distinct_LarsCode(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
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
            allStandards.AddRange(shortCourseStandards);

            // set up active version 
            var newActiveVersion = activeValidApprenticeshipStandards[0];
            newActiveVersion.IfateReferenceNumber = "ST0001";
            newActiveVersion.Version = "2";
            newActiveVersion.LarsCode = "100002";

            // add a retired version to have same IfateReferenceNumber and different LarsCode
            allStandards.Add(activeValidApprenticeshipStandards.Select(x => new Standard { IfateReferenceNumber = x.IfateReferenceNumber, Status = "Retired", LarsCode = "100001", Version = "1", LarsStandard = newActiveVersion.LarsStandard }).First());

            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            // Act
            var count = await repository.Count(StandardFilter.Active, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(activeValidApprenticeshipStandards.Count + activeInvalidApprenticeshipStandards.Count + retiredApprenticeshipStandards.Count + 1);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_NotYetApproved_Standards(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
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
            allStandards.AddRange(shortCourseStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            // Act
            var count = await repository.Count(StandardFilter.NotYetApproved, CourseType.Apprenticeship);

            // Assert
            count.Should().Be(notYetApprovedApprenticeshipStandards.Count);
        }

        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_All_Standards(
            [ApprenticeshipStandardsLarsValid] List<Standard> activeValidApprenticeshipStandards,
            [ApprenticeshipStandardsNotLarsValid] List<Standard> activeInvalidApprenticeshipStandards,
            [ApprenticeshipStandardsNotYetApproved] List<Standard> notYetApprovedApprenticeshipStandards,
            [ApprenticeshipStandardsWithdrawn] List<Standard> withdrawnApprenticeshipStandards,
            [ApprenticeshipStandardsRetired] List<Standard> retiredApprenticeshipStandards,
            [ShortCourseStandards] List<Standard> shortCourseStandards,
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var allStandards = new List<Standard>();
            allStandards.AddRange(activeValidApprenticeshipStandards);
            allStandards.AddRange(activeInvalidApprenticeshipStandards);
            allStandards.AddRange(notYetApprovedApprenticeshipStandards);
            allStandards.AddRange(withdrawnApprenticeshipStandards);
            allStandards.AddRange(retiredApprenticeshipStandards);
            allStandards.AddRange(shortCourseStandards);
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(allStandards);

            var count = await repository.Count(StandardFilter.None, CourseType.Apprenticeship);

            var total = activeInvalidApprenticeshipStandards.Count + activeInvalidApprenticeshipStandards.Count + notYetApprovedApprenticeshipStandards.Count
                + withdrawnApprenticeshipStandards.Count + retiredApprenticeshipStandards.Count;
            count.Should().Be(total);
        }
    }
}
