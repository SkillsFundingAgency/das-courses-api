using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardsService
    {
        Task<GetStandardsListResult> GetStandardsList(string keyword);
        Task<Standard> GetStandard(int standardId);
    }
}
