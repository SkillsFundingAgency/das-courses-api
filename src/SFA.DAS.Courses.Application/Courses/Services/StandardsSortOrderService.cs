using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class StandardsSortOrderService : IStandardsSortOrderService
    {
        public IOrderedEnumerable<Standard> OrderBy(IEnumerable<Standard> standards, OrderBy orderBy)
        {
            return standards.OrderBy(standard => standard.Id);
        }
    }
}
