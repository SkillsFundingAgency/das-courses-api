using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities.Versioning;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IVersioningStandardRepository
    {
        Task<Standard> GetStandardByUId(string standardUId);
        Task InsertMany(IEnumerable<Standard> standards, IEnumerable<StandardAdditionalInformation> additionalInformation);
    }
}
