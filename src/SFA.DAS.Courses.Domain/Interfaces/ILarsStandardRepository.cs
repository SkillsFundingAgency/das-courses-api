using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILarsStandardRepository
    {
        Task InsertMany(IEnumerable<LarsStandard> larsStandardImports);
        Task DeleteAll();
    }
}
