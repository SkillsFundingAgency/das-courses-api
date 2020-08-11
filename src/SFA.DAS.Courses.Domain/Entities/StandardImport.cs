using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        public static implicit operator StandardImport(Domain.ImportTypes.Standard standard)
        {
            //TODO
            var coreSkillsCount = standard.Duties.Any() ? string.Join("|", standard.Duties.Where(c=>c.IsThisACoreDuty.Equals(1)).Select(c=>c.DutyDetail)) : null;
            
            return new StandardImport
            {
                Id = standard.LarsCode,
                CoreSkillsCount = coreSkillsCount,
                IntegratedDegree = standard.IntegratedDegree,
                Level = standard.Level,
                OverviewOfRole = standard.OverviewOfRole,
                StandardPageUrl = standard.StandardPageUrl.AbsoluteUri,
                Title = standard.Title,
                TypicalDuration = standard.TypicalDuration,
                TypicalJobTitles = string.Join("|", standard.TypicalJobTitles),
                Version = standard.Version,
                Keywords = standard.Keywords.Any() ? string.Join("|",standard.Keywords) : null,
                RouteId = standard.RouteId
            };
        }
    }
}