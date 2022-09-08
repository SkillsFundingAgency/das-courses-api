using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public static class GetStandard
    {
        public static async Task<Standard> ByAnyId(IStandardsService service, string id)
        {
            if (IsLarsCode(id, out var larsCode))
            {
                return await service.GetLatestActiveStandard(larsCode);
            }
            else if (IsIfateReference(id))
            {
                return await service.GetLatestActiveStandard(id);
            }
            else
            {
                return await service.GetStandard(id);
            }
        }

        private static bool IsIfateReference(string id)
            => id.Length == 6;

        private static bool IsLarsCode(string id, out int larsCode)
            => int.TryParse(id, out larsCode);
    }
}
