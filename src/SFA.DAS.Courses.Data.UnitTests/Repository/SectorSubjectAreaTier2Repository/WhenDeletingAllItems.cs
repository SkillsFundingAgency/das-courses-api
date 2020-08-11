using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.SectorSubjectAreaTier2Repository
{
    public class WhenDeletingAllItems
    {
        private List<SectorSubjectAreaTier2> _sectorSubjectAreaTier2s;  
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.SectorSubjectAreaTier2Repository _sectorSubjectAreaTier2Repository;

        [SetUp]
        public void Arrange()
        {
            _sectorSubjectAreaTier2s = new List<SectorSubjectAreaTier2>
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
            _coursesDataContext.Setup(x => x.SectorSubjectAreaTier2).ReturnsDbSet(_sectorSubjectAreaTier2s);
            
            _sectorSubjectAreaTier2Repository = new Data.Repository.SectorSubjectAreaTier2Repository(_coursesDataContext.Object);
        }

        [Test]
        public void Then_The_SectorSubjectAreaTier2_Items_Are_Removed()
        {
            //Act
            _sectorSubjectAreaTier2Repository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.SectorSubjectAreaTier2.RemoveRange(_coursesDataContext.Object.SectorSubjectAreaTier2), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChanges(), Times.Once);
        }
    }
}