using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorSubjectAreaTier2ImportRepository
{
    public class WhenGettingAllItems
    {
        private List<SectorSubjectAreaTier2Import> _sectorSubjectAreaTier2Imports;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorSubjectAreaTier2ImportRepository _sectorSubjectAreaTier2ImportRepository;

        [SetUp]
        public void Arrange()
        {
            _sectorSubjectAreaTier2Imports = new List<SectorSubjectAreaTier2Import>
            {
                new SectorSubjectAreaTier2Import
                {
                    SectorSubjectAreaTier2 = 10.1m
                },
                new SectorSubjectAreaTier2Import
                {
                    SectorSubjectAreaTier2 = 10.2m
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.SectorSubjectAreaTier2Import).ReturnsDbSet(_sectorSubjectAreaTier2Imports);
            
            _sectorSubjectAreaTier2ImportRepository = new Data.Repository.SectorSubjectAreaTier2ImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_SectorSubjectAreaTier2Import_Items_Are_Returned()
        {
            //Act
            var actual = await _sectorSubjectAreaTier2ImportRepository.GetAll();
            
            //Assert
            Assert.IsNotNull(actual);
            actual.Should().BeEquivalentTo(_sectorSubjectAreaTier2Imports);
        }
    }
}