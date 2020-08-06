using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IQualificationSectorSubjectAreaService
    {
        Task<QualificationItem> GetEntry(string itemHash);
        Task<IEnumerable<QualificationItemList>> GetEntries();
    }
}