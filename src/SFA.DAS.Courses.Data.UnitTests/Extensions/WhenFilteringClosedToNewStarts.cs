using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Courses.Domain.TestHelper.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Extensions
{
    public class WhenFilteringClosedToNewStarts
    {
        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
        public void Then_Apprenticeships_With_Null_LastDateStarts_Are_Not_Returned(
            List<Standard> standards)
        {
            // Arrange
            foreach (var standard in standards)
            {
                standard.LarsStandard.LastDateStarts = null;
            }

            // Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            // Assert
            actual.Should().BeEmpty();
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
        public void Then_Apprenticeships_With_Past_LastDateStarts_Are_Returned(
            List<Standard> standards)
        {
            // Arrange
            var now = DateTime.UtcNow;

            foreach (var standard in standards)
            {
                standard.LarsStandard.LastDateStarts = now.AddDays(-1);
            }

            // Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            // Assert
            actual.Should().BeEquivalentTo(standards);
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.Apprenticeship)]
        public void Then_Apprenticeships_With_Future_LastDateStarts_Are_Not_Returned(
            List<Standard> standards)
        {
            // Arrange
            var now = DateTime.UtcNow;

            foreach (var standard in standards)
            {
                standard.LarsStandard.LastDateStarts = now.AddDays(1);
            }

            // Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            // Assert
            actual.Should().BeEmpty();
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.ApprenticeshipUnit)]
        public void Then_ShortCourses_With_Null_LastDateStarts_Are_Not_Returned(
            List<Standard> standards)
        {
            // Arrange
            foreach (var standard in standards)
            {
                standard.ShortCourseDates.LastDateStarts = null;
            }

            // Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            // Assert
            actual.Should().BeEmpty();
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.ApprenticeshipUnit)]
        public void Then_ShortCourses_With_Past_LastDateStarts_Are_Returned(
            List<Standard> standards)
        {
            // Arrange
            var now = DateTime.UtcNow;

            foreach (var standard in standards)
            {
                standard.ShortCourseDates.LastDateStarts = now.AddDays(-1);
            }

            // Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            // Assert
            actual.Should().BeEquivalentTo(standards);
        }

        [Test, StandardInlineAutoData(ApprenticeshipType.ApprenticeshipUnit)]
        public void Then_ShortCourses_With_Future_LastDateStarts_Are_Not_Returned(
            List<Standard> standards)
        {
            // Arrange
            var now = DateTime.UtcNow;

            foreach (var standard in standards)
            {
                standard.ShortCourseDates.LastDateStarts = now.AddDays(1);
            }

            // Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            // Assert
            actual.Should().BeEmpty();
        }
    }
}
