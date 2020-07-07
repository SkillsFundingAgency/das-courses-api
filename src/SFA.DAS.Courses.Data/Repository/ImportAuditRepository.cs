using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Data.Repository
{
    public class ImportAuditRepository : IImportAuditRepository
    {
        private readonly ICoursesDataContext _dataContext;

        public ImportAuditRepository (ICoursesDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Insert(ImportAudit importAudit)
        {
            await _dataContext.ImportAudit.AddAsync(importAudit);
            _dataContext.SaveChanges();
        }

        public async Task<ImportAudit> GetLastImportByType(ImportType importType)
        {
            var record = await _dataContext
                .ImportAudit
                .OrderByDescending(c => c.TimeStarted)
                .FirstOrDefaultAsync(c => c.ImportType.Equals(importType));

            return record;
        }
    }
}
