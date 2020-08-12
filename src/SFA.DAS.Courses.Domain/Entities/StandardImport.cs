using System;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardImport : StandardBase
    {
        public static implicit operator StandardImport(Domain.ImportTypes.Standard standard)
        {
            string coreSkillsCount = null;

            //Check duties and skills not empty
            if (standard.Duties.Any() && standard.Skills.Any())
            {
                if(standard.CoreAndOptions)
                {   
                    //Put all of the MappedSkills skillIds from all of the core duties into a list
                    var mappedSkillsList = standard.Duties.Where(d => d.IsThisACoreDuty.Equals(1)).SelectMany(d => d.MappedSkills).ToList();
                    //Store the detail property for each skill in standard.Skills who's skillId appears in the list of mapped skills for core duties
                    coreSkillsCount = string.Join("|", standard.Skills.Where(s => mappedSkillsList.Contains(s.SkillId)).Select(s => s.Detail));
                } else
                {   
                    //Stores all skill details from skills in standard.Skills if CoreAndOptions false
                    coreSkillsCount = string.Join("|", standard.Skills.Select(s => s.Detail));
                }
            }
            

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
                Keywords = standard.Keywords.Any() ? string.Join("|", standard.Keywords) : null,
                RouteId = standard.RouteId
            };
        }
    }
}
