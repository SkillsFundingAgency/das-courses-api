using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardStagingRepository
    {
        Task InsertMany(IEnumerable<StandardStaging> standardsStaging);
        void DeleteAll();
    }
}
