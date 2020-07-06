using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.StandardRepository
{
    public class WhenGettingCount
    {
        [Test, RecursiveMoqAutoData]
        public async Task Then_Gets_Count_From_Context_Of_Available_Standards(
            
            [Frozen] Mock<ICoursesDataContext> mockDataContext,
            Data.Repository.StandardRepository repository)
        {
            var standards = new List<Standard>
            {
                new Standard()
                {
                    Id = 1,
                    LarsStandard = 
                        new LarsStandard
                        {
                            EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                            LastDateStarts = null
                        }
                    
                },
                new Standard
                {
                    Id = 2,
                    LarsStandard = 
                        new LarsStandard
                        {
                            EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                            LastDateStarts = null
                        }
                    
                },
                new Standard
                {
                    Id = 3,
                    LarsStandard = 
                        new LarsStandard
                        {
                            EffectiveFrom = DateTime.UtcNow.AddDays(1),
                            LastDateStarts = null
                        }
                    
                }
            };
            
            mockDataContext
                .Setup(context => context.Standards)
                .ReturnsDbSet(standards);

            var count = await repository.Count();

            count.Should().Be(standards.Count - 1);
        }
    }
}
