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
    public class WhenNoFilterIsSpecified
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
                    Title = "Not Yet Approved",
                    Status = "In development",
                },
                new Standard
                {
                    Title = "Not Yet Approved",
                    Status = "Proposal in development",
                },
                new Standard
                {
                    Title = "Withdrawn",
                    Status = "Withdrawn",
                },
                new Standard
                {
                    Title = "Retired",
                    Status = "Retired",
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
            var actual = standards.FilterStandards(StandardFilter.None);

            //Assert
            actual.Count().Should().Be(7);
        }
    }
}
