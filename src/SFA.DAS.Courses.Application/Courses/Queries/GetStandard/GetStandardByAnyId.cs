using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public class GetStandardByAnyId
    {
        private readonly IStandardsService _standardsService;

        public GetStandardByAnyId(IStandardsService standardsService)
        {
            this._standardsService = standardsService;
        }

        public async Task<Standard> GetStandard(string id)
        {
            if (IsLarsCode(id, out var larsCode))
            {
                return await _standardsService.GetLatestActiveStandard(larsCode);
            }
            else if (IsIfateReference(id))
            {
                return await _standardsService.GetLatestActiveStandard(id);
            }
            else
            {
                return await _standardsService.GetStandard(id);
            }
        }

        private static bool IsIfateReference(string id)
            => id.Length == 6;

        private static bool IsLarsCode(string id, out int larsCode)
            => int.TryParse(id, out larsCode);
    }
}
