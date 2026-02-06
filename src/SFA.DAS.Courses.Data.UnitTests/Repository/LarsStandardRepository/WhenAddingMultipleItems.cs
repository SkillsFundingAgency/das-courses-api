using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.LarsStandardRepository
{
    public class WhenAddingMultipleItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private Data.Repository.LarsStandardRepository _larsStandardRepository;
        private List<LarsStandard> _larsStandards;

        [SetUp]
        public void Arrange()
        {
            _larsStandards = new List<LarsStandard>
            {
                new LarsStandard
                {
                    LarsCode = "1"
                },
                new LarsStandard
                {
                    LarsCode = "2"
                }
            };

            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.LarsStandards).ReturnsDbSet(new List<LarsStandard>());
            _larsStandardRepository = new Data.Repository.LarsStandardRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_LarsStandard_Items_Are_Added()
        {
            //Act
            await _larsStandardRepository.InsertMany(_larsStandards);
            
            //Assert
            _coursesDataContext.Verify(x=>x.LarsStandards.AddRangeAsync(_larsStandards, It.IsAny<CancellationToken>()), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
