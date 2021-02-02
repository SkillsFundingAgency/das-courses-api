using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Courses.Data.Extensions;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Data.UnitTests.Extensions
{
    public class WhenFilteringActiveStandards
    {
        [Test]
        public void Then_All_Standards_Are_Returned()
        {
            //Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = sameDate,
                        LastDateStarts = sameDate
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = DateTime.UtcNow.AddDays(-1)
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(1),
                        LastDateStarts = null
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard =
                        new LarsStandard
                        {
                            EffectiveFrom = DateTime.UtcNow.AddMonths(-2),
                            LastDateStarts = DateTime.UtcNow.AddMonths(-1)
                        },
                    Status = "Approved for delivery"
                }
            }.AsQueryable();

            //Act
            var actual = standards.FilterStandards(StandardFilter.Active);

            //Assert
            actual.Count().Should().Be(4);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_All_Standards_With_Lars_Records_Are_Returned()
        {
            //Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = sameDate,
                        LastDateStarts = sameDate
                    },
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = null,
                    Status = "Approved for delivery"
                },
                new Standard
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard =
                        new LarsStandard
                        {
                            EffectiveFrom = DateTime.UtcNow.AddMonths(-2),
                            LastDateStarts = DateTime.UtcNow.AddMonths(-1)
                        },
                    Status = "Approved for delivery"
                }
            }.AsQueryable();

            //Act
            var actual = standards.FilterStandards(StandardFilter.Active);

            //Assert
            actual.Count().Should().Be(2);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

    }
}
