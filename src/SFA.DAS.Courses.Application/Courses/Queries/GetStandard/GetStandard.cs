using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Identifiers;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetStandard
{
    public static class GetStandard
    {
        public static async Task<Standard> ByAnyId(IStandardsService service, string id)
        {
            if (IsLarsCode(id))
            {
                return await service.GetLatestActiveStandard(id);
            }
            else if (IsStandardReference(id))
            {
                return await service.GetLatestActiveStandardByIfateReferenceNumber(id);
            }
            else
            {
                return await service.GetStandard(id);
            }
        }

        private static bool IsStandardReference(string id)
            => IdentifierRegexes.StandardReferenceNumber.IsMatch(id) ||
               IdentifierRegexes.FoundationReferenceNumber.IsMatch(id) ||
               IdentifierRegexes.ShortCourseReferenceNumber.IsMatch(id);

        private static bool IsLarsCode(string id)
            => int.TryParse(id, out _)
            || IdentifierRegexes.ShortCourseLarsCode.IsMatch(id);
    }
}
