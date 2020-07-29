using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworkFundingRepository
    {
        Task InsertMany(IEnumerable<FrameworkFunding> frameworkFunding);
        void DeleteAll();
    }
}