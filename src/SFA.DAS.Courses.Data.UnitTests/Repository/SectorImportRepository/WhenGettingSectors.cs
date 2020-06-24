using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorImportRepository
{
    public class WhenGettingSectors
    {
        private List<SectorImport> _sectorsImport;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorImportRepository _sectorImportRepository;

        [SetUp]
        public void Arrange()
        {
            _sectorsImport = new List<SectorImport>
            {
                new SectorImport
                {
                    Id = Guid.NewGuid()
                },
                new SectorImport
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.SectorsImport).ReturnsDbSet(_sectorsImport);
            
            _sectorImportRepository = new Data.Repository.SectorImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_SectorsImport_Are_Returned()
        {
            //Act
            var sectorsImport = await _sectorImportRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(sectorsImport);
            sectorsImport.Should().BeEquivalentTo(_sectorsImport);
        }
    }
}