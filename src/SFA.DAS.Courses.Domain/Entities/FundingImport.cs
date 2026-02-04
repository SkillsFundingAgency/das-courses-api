using System;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class FundingImport
    {
        public Guid Id { get; set; }
        public string LearnAimRef { get; set; }
        public string FundingCategory { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal RateWeighted { get; set; }
        public decimal RateUnWeighted { get; set; }
        public string WeightingFactor { get; set; }
        public string AdultSkillsFundingBand { get; set; }
        public int? FundedGuidedLearningHours { get; set; }
        
        public static implicit operator FundingImport(FundingCsv source)
        {
            if (source == null)
                return null;

            return new FundingImport
            {
                Id = Guid.NewGuid(),
                LearnAimRef = source.LearnAimRef,
                FundingCategory = source.FundingCategory,
                EffectiveFrom = source.EffectiveFrom,
                EffectiveTo = source.EffectiveTo,
                RateWeighted = source.RateWeighted,
                RateUnWeighted = source.RateUnWeighted,
                WeightingFactor = source.WeightingFactor,
                AdultSkillsFundingBand = source.AdultSkillsFundingBand,
                FundedGuidedLearningHours = source.FundedGuidedLearningHours,
            };
        }
    }
}
