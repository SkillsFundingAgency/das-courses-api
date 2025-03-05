using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorSubjectAreaTier2ImportRepository
{
    public class WhenAddingMultipleItems
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
            _coursesDataContext.Setup(x => x.SectorSubjectAreaTier2Import).ReturnsDbSet(new List<SectorSubjectAreaTier2Import>());
            
            _sectorSubjectAreaTier2ImportRepository = new Data.Repository.SectorSubjectAreaTier2ImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_SectorSubjectAreaTier2Import_Items_Are_Added()
        {
            //Act
            await _sectorSubjectAreaTier2ImportRepository.InsertMany(_sectorSubjectAreaTier2Imports);
            
            //Assert
            _coursesDataContext.Verify(x=>x.SectorSubjectAreaTier2Import.AddRangeAsync(_sectorSubjectAreaTier2Imports, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
