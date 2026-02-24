using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.ImportTypes.SkillsEngland
{
    public sealed class SkillsEnglandStandardsResult
    {
        public IEnumerable<Apprenticeship> Apprenticeships { get; init; }
        public IEnumerable<FoundationApprenticeship> FoundationApprenticeships { get; init; }
        public IEnumerable<ApprenticeshipUnit> ApprenticeshipUnits { get; init; }
    }

}
