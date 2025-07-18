﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.Courses.Data.UnitTests.DatabaseMock;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Repository.ApprenticeshipFundingImportRepository
{
    public class WhenDeletingAllItems
    {
        private Mock<ICoursesDataContext> _coursesDataContext;
        private List<ApprenticeshipFundingImport> _apprenticeshipFundingImports;
        private Data.Repository.ApprenticeshipFundingImportRepository _apprenticeshipFundingImportRepository;

        [SetUp]
        public void Arrange()
        {
            _apprenticeshipFundingImports = new List<ApprenticeshipFundingImport>
            {
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid()
                },
                new ApprenticeshipFundingImport
                {
                    Id = Guid.NewGuid()
                }
            };
            
            _coursesDataContext = new Mock<ICoursesDataContext>();
            _coursesDataContext.Setup(x => x.ApprenticeshipFundingImport).ReturnsDbSet(_apprenticeshipFundingImports);
            

            _apprenticeshipFundingImportRepository = new Data.Repository.ApprenticeshipFundingImportRepository(_coursesDataContext.Object);
        }

        [Test]
        public async Task Then_The_ApprenticeshipFundingImport_Items_Are_Removed()
        {
            //Act
            await _apprenticeshipFundingImportRepository.DeleteAll();
            
            //Assert
            _coursesDataContext.Verify(x=>x.ApprenticeshipFundingImport.RemoveRange(_coursesDataContext.Object.ApprenticeshipFundingImport), Times.Once);
            _coursesDataContext.Verify(x=>x.SaveChangesAsync(default), Times.Once);
        }
    }
}
