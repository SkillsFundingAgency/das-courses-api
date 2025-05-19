using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IApprenticeshipFundingRepository
    {
        Task InsertMany(IEnumerable<ApprenticeshipFunding> apprenticeshipFunding);
        Task DeleteAll();
    }
}
