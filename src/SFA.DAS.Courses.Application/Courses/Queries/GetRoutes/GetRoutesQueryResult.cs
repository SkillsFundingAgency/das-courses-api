using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetRoutes
{
    public class GetRoutesQueryResult
    {
        public IEnumerable<Route> Routes { get; set; }
    }
}