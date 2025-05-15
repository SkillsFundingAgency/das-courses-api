using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorSubjectAreaTier2Repository
{
    public class WhenAddingMultipleItems
    {
        private List<SectorSubjectAreaTier2> _sectorSubjectAreaTier2Items;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorSubjectAreaTier2Repository _sectorSubjectAreaTier2Repository;

        [SetUp]
        public void Arrange()
        {
            _sectorSubjectAreaTier2Items = new List<SectorSubjectAreaTier2>
            {
                new SectorSubjectAreaTier2
                {
                    SectorSubjectAreaTier2 = 10.1m
                },
                new SectorSubjectAreaTier2
                {
                    SectorSubjectAreaTier2 = 10.2m
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.SectorSubjectAreaTier2).ReturnsDbSet(new List<SectorSubjectAreaTier2>());
            
            _sectorSubjectAreaTier2Repository = new Data.Repository.SectorSubjectAreaTier2Repository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_SectorSubjectAreaTier2_Items_Are_Added()
        {
            //Act
            await _sectorSubjectAreaTier2Repository.InsertMany(_sectorSubjectAreaTier2Items);
            
            //Assert
            _coursesDataContext.Verify(x=>x.SectorSubjectAreaTier2.AddRangeAsync(_sectorSubjectAreaTier2Items, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
