using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingShortCourseDates
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.StandardRepository _standardRepository;

        [SetUp]
        public void Arrange()
        {
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(c => c.ApprenticeshipFunding).ReturnsDbSet(new List<ApprenticeshipFunding>());
            _standardRepository = new Data.Repository.StandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_Returns_Grouped_ShortCourseDates_By_LarsCode()
        {
            // Arrange
            var standards = new List<Standard>
            {
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = "ZSC00001",
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    ApprovedForDelivery = new DateTime(2024, 1, 1),
                    VersionLatestStartDate = new DateTime(2024, 6, 1)
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = "ZSC00001",
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 1,
                    ApprovedForDelivery = new DateTime(2024, 2, 1),
                    VersionLatestStartDate = new DateTime(2024, 7, 1)
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = "ZSC00002",
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    ApprovedForDelivery = new DateTime(2024, 3, 1),
                    VersionLatestStartDate = new DateTime(2024, 8, 1)
                }
            };

            _coursesDataContext.Setup(c => c.Standards).ReturnsDbSet(standards);

            // Act
            var result = await _standardRepository.GetShortCourseDates();

            // Assert
            result.Should().HaveCount(2);

            var first = result.Single(x => x.LarsCode == "ZSC00001");
            first.EffectiveFrom.Should().Be(new DateTime(2024, 1, 1));
            first.EffectiveTo.Should().Be(new DateTime(2024, 7, 1));
            first.LastDateStarts.Should().Be(new DateTime(2024, 7, 1));

            var second = result.Single(x => x.LarsCode == "ZSC00002");
            second.EffectiveFrom.Should().Be(new DateTime(2024, 3, 1));
            second.EffectiveTo.Should().Be(new DateTime(2024, 8, 1));
            second.LastDateStarts.Should().Be(new DateTime(2024, 8, 1));
        }

        [Test]
        public async Task Then_Filters_By_LarsCode_When_Provided()
        {
            // Arrange
            var standards = new List<Standard>
            {
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = "ZSC00001",
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    ApprovedForDelivery = new DateTime(2024, 1, 1),
                    VersionLatestStartDate = new DateTime(2024, 6, 1)
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = "ZSC00002",
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    ApprovedForDelivery = new DateTime(2024, 3, 1),
                    VersionLatestStartDate = new DateTime(2024, 8, 1)
                }
            };

            _coursesDataContext.Setup(c => c.Standards).ReturnsDbSet(standards);

            // Act
            var result = await _standardRepository.GetShortCourseDates("ZSC00001");

            // Assert
            result.Should().HaveCount(1);
            result.Single().LarsCode.Should().Be("ZSC00001");
        }

        [Test]
        public async Task Then_Ignores_ShortCourses_With_Empty_LarsCode()
        {
            // Arrange
            var standards = new List<Standard>
            {
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = string.Empty,
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    ApprovedForDelivery = new DateTime(2024, 1, 1),
                    VersionLatestStartDate = new DateTime(2024, 6, 1)
                },
                new Standard
                {
                    CourseType = CourseType.ShortCourse,
                    ApprenticeshipType = ApprenticeshipType.ApprenticeshipUnit,
                    LarsCode = "ZSC00001",
                    Status = "Approved for delivery",
                    VersionMajor = 1,
                    VersionMinor = 0,
                    ApprovedForDelivery = new DateTime(2024, 2, 1),
                    VersionLatestStartDate = new DateTime(2024, 7, 1)
                }
            };

            _coursesDataContext.Setup(c => c.Standards).ReturnsDbSet(standards);

            // Act
            var result = await _standardRepository.GetShortCourseDates();

            // Assert
            result.Should().HaveCount(1);
            result.Single().LarsCode.Should().Be("ZSC00001");
        }
    }
}
