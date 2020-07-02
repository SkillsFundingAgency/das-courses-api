using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetSectors
{
    public class GetSectorsListResult
    {
        public IEnumerable<Sector> Sectors { get ; set ; }
    }
}