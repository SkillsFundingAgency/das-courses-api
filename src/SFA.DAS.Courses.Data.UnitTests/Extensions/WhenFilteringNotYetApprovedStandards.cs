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
    public class WhenFilteringNotYetApprovedStandards
    {
        [Test]
        public void Then_In_Development_Standards_Are_Returned()
        {
            //Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                new Standard
                {
                    LarsCode = "0",
                    Title = "Not Yet Approved",
                    Status = "In development",
                },
                new Standard
                {
                    LarsCode = "0",
                    Title = "Not Yet Approved",
                    Status = "Proposal in development",
                },
                new Standard
                {
                    LarsCode = "1",
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
                    LarsCode = "2",
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
                    LarsCode = "3",
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
            var actual = standards.FilterStandards(StandardFilter.NotYetApproved);

            //Assert
            actual.Count().Should().Be(2);
            actual.ToList().TrueForAll(c => c.Title.Equals("Not Yet Approved"));
        }
    }
}
