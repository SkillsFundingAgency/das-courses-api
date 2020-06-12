using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IImportAuditRepository
    {
        Task Insert(ImportAudit importAudit);
    }
}