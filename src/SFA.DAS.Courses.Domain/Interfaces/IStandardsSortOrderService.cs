using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IStandardsSortOrderService
    {
        IOrderedEnumerable<Entities.Standard> OrderBy(IEnumerable<Entities.Standard> standards,
            OrderBy orderBy);
    }
}
