using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;
using SFA.DAS.Courses.Domain.Search;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class StandardsSortOrderService : IStandardsSortOrderService
    {
        public IOrderedEnumerable<Standard> OrderBy(IEnumerable<Standard> standards, OrderBy orderBy, string keyword)
        {
            if (string.IsNullOrEmpty(keyword) || orderBy == Domain.Search.OrderBy.Title)
            {
                return OrderByTitle(standards);
            }
            return OrderByScore(standards);
        }
        
        private IOrderedEnumerable<Standard> OrderByScore(IEnumerable<Standard> standards)
        {
            return standards.OrderByDescending(c => c.SearchScore)
                .ThenBy(c => c.Title)
                .ThenBy(c => c.Level)
                .ThenBy(c => c.LarsStandard != null ? c.LarsStandard.LarsCode : int.MaxValue);
        }

        private IOrderedEnumerable<Standard> OrderByTitle(IEnumerable<Standard> standards)
        {
            return standards.OrderBy(c => c.Title)
                .ThenBy(c => c.Level)
                .ThenBy(c => c.LarsStandard != null ? c.LarsStandard.LarsCode : int.MaxValue);
        }
    }
}
