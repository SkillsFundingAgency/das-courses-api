﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class SectorSubjectAreaTier2ImportRepository : ISectorSubjectAreaTier2ImportRepository
    {
        private readonly ICoursesDataContext _coursesDataContext;
        public SectorSubjectAreaTier2ImportRepository (ICoursesDataContext coursesDataContext)
        {
            _coursesDataContext = coursesDataContext;
        }

        public async Task<IEnumerable<SectorSubjectAreaTier2Import>> GetAll()
        {
            var items = await _coursesDataContext.SectorSubjectAreaTier2Import.ToListAsync();
            return items;
        }

        public async Task DeleteAll()
        {
            _coursesDataContext.SectorSubjectAreaTier2Import.RemoveRange(_coursesDataContext.SectorSubjectAreaTier2Import);
            await _coursesDataContext.SaveChangesAsync();
        }

        public async Task InsertMany(IEnumerable<SectorSubjectAreaTier2Import> sectorSubjectAreaTier2Imports)
        {
            await _coursesDataContext.SectorSubjectAreaTier2Import.AddRangeAsync(sectorSubjectAreaTier2Imports);
            await _coursesDataContext.SaveChangesAsync();
        }
    }
}
