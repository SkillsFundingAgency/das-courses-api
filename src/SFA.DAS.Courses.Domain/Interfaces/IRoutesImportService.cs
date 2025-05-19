using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface IRoutesImportService
    {
        List<RouteImport> GetDistinctRoutesFromStandards(List<ImportTypes.Standard> standards);
    }
}
