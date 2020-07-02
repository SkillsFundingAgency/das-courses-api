using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorRepository
{
    public class WhenGettingSectors
    {
        private List<Sector> _sectors;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorRepository _sectorRepository;

        [SetUp]
        public void Arrange()
        {
            _sectors = new List<Sector>
            {
                new Sector()
                {
                    Id = Guid.NewGuid(),
                    Route = "Second"
                },
                new Sector
                {
                    Id = Guid.NewGuid(),
                    Route="First"
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.Sectors).ReturnsDbSet(_sectors);
            
            _sectorRepository = new Data.Repository.SectorRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_Sectors_Are_Returned()
        {
            //Act
            var sectors = await _sectorRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(sectors);
            sectors.Should().BeEquivalentTo(_sectors.OrderBy(c=>c.Route), options=>options.WithStrictOrdering());
        }
    }
}