using System.Threading.Tasks;
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
    }
}