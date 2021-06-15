using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Extensions
{
    public class WhenFilteringClosedToNewStarts
    {
        [Test, RecursiveMoqAutoData]
        public void Then_Standards_With_Null_LastDateStarts_Are_Not_Returned(
            List<Standard> standards)
        {
            //Arrange
            foreach (var standard in standards)
            {
                standard.LarsStandard.LastDateStarts = null;
            }

            //Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            //Assert
            actual.Should().BeEmpty();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Standards_With_Past_LastDateStarts_Are_Returned(
            List<Standard> standards)
        {
            //Arrange
            foreach (var standard in standards)
            {
                standard.LarsStandard.LastDateStarts = DateTime.UtcNow.AddDays(-1);
            }

            //Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            //Assert
            actual.Should().BeEquivalentTo(standards);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Standards_With_Future_LastDateStarts_Are_Not_Returned(
            List<Standard> standards)
        {
            //Arrange
            foreach (var standard in standards)
            {
                standard.LarsStandard.LastDateStarts = DateTime.UtcNow.AddDays(1);
            }

            //Act
            var actual = standards
                .AsQueryable()
                .FilterStandards(StandardFilter.ClosedToNewStarts);

            //Assert
            actual.Should().BeEmpty();
        }
    }
}
