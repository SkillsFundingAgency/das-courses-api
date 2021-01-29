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
    public class WhenFilteringAvailableStandards
    {
        [Test]
        public void Then_Standards_With_Null_LastDateStarts_Are_Returned()
        {
            //Arrange
            var standards = new List<Standard>
            {
                ValidStandard(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = 
                    new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-2),
                        LastDateStarts = DateTime.UtcNow.AddMonths(-1)
                    }
                
                }
            }.AsQueryable();

            //Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable);

            //Assert
            actual.Count().Should().Be(1);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_Standards_With_A_Future_Start_Date_Are_Not_Returned()
        {
            //Arrange
            var standards = new List<Standard>
            {
                ValidStandard(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                                    {
                                        EffectiveFrom = DateTime.UtcNow.AddMonths(1),
                                        LastDateStarts = null
                                    }
                    
                }
            }.AsQueryable();
            
            //Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable);

            //Assert
            actual.Count().Should().Be(1);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_Standards_With_LastDateStarts_In_The_Past_Are_Not_Returned()
        {
            //Arrange
            var standards = new List<Standard>
            {
                ValidStandard(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                        {
                            EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                            LastDateStarts = DateTime.UtcNow.AddDays(-1)
                        }
                    
                }
            }.AsQueryable();
            
            //Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable);

            //Assert
            actual.Count().Should().Be(1);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_Standards_With_The_Same_EffectiveFrom_And_LastDateStarts_Are_Not_Returned()
        {
            //Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                ValidStandard(),
                new Standard
                {
                    Title = "Not Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    LarsStandard = new LarsStandard
                        {
                            EffectiveFrom = sameDate,
                            LastDateStarts = sameDate
                        }
                    
                }
            }.AsQueryable();
            
            //Act
            var actual = standards.FilterStandards(StandardFilter.ActiveAvailable);

            //Assert
            actual.Count().Should().Be(1);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        [Test]
        public void Then_If_The_Filter_Param_Is_Active_Then_All_Standards_Are_Returned()
        {
            //Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                ValidStandard(),
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
            actual.Count().Should().Be(5);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }
        
        [Test]
        public void Then_If_The_Filter_Param_Is_Active_Then_All_Standards_With_Lars_Records_Are_Returned()
        {
            //Arrange
            var sameDate = DateTime.UtcNow;
            var standards = new List<Standard>
            {
                ValidStandard(),
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
            actual.Count().Should().Be(3);
            actual.ToList().TrueForAll(c => c.Title.Equals("Available"));
        }

        private static Standard ValidStandard()
        {
            return new Standard
            {
                Title = "Available",
                ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                LarsStandard = new LarsStandard
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = null
                    },
                Status = "Approved for delivery"
                
            };
        }
    }
    
}
