using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardRepository
    {
        Task<IEnumerable<Standard>> GetAll();
        void DeleteAll();
        Task InsertMany(IEnumerable<Standard> standards);
        Task<Standard> Get(int id);
    }
}