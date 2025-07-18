﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class FrameworkFundingImportRepository : IFrameworkFundingImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;

        public FrameworkFundingImportRepository(ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }
        public async Task InsertMany(IEnumerable<FrameworkFundingImport> frameworkFundingImports)
        {
            await _coursesDataContext.FrameworkFundingImport.AddRangeAsync(frameworkFundingImports);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.FrameworkFundingImport.RemoveRange(_coursesDataContext.FrameworkFundingImport);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<FrameworkFundingImport>> GetAll()
        {
            var frameworkFundingImportIems = await _coursesDataContext.FrameworkFundingImport.ToListAsync();
            return frameworkFundingImportIems;
        }
    }
}
