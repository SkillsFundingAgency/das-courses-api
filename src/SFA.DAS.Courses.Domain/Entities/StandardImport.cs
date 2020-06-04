using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : Standard
    {
        public static implicit operator StandardImport(Domain.ImportTypes.Standard standard)
        {
            return new StandardImport
            {
                Id = standard.LarsCode,
                CoreSkillsCount = standard.Duties.Any() ? standard.Duties.Where(c=>c.IsThisACoreDuty.Equals(1)).Select(c=>c.DutyDetail).Join("|") : null,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                MaxFunding = standard.MaxFunding,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl.AbsoluteUri,
                Route = standard.Route,
                Title = standard.Title,
                TypicalDuration = standard.TypicalDuration,
                TypicalJobTitles = standard.TypicalJobTitles.Join(","),
                Version = standard.Version
            };
        }
    }
}