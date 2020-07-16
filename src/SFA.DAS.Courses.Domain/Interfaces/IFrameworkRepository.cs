using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IFrameworkRepository
    {
        Task InsertMany(IEnumerable<Framework> frameworks);
        void DeleteAll();
        Task<Framework> Get(string id);
        Task<IEnumerable<Framework>> GetAll();
    }
}