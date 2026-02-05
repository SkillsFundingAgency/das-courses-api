using System.Linq;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Domain.Policies
{
    public static class AllowedApprenticeshipTypesPolicy
    {
        private static readonly ApprenticeshipType[] StandardTypes =
        {
            ApprenticeshipType.Apprenticeship,
            ApprenticeshipType.FoundationApprenticeship
        };

        public static bool IsStandard(ApprenticeshipType type) =>
            StandardTypes.Contains(type);
    }
}
