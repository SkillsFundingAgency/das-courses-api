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
        
        public static implicit operator FundingImport(FundingCsv fundingCsv)
        {
            return new FundingImport
            {
                Id = Guid.NewGuid(),
                LearnAimRef = fundingCsv.LearnAimRef,
                FundingCategory = fundingCsv.FundingCategory,
                EffectiveFrom = fundingCsv.EffectiveFrom,
                EffectiveTo = fundingCsv.EffectiveTo,
                RateWeighted = fundingCsv.RateWeighted,
                RateUnWeighted = fundingCsv.RateUnWeighted,
                WeightingFactor = fundingCsv.WeightingFactor,
                AdultSkillsFundingBand = fundingCsv.AdultSkillsFundingBand,
                FundedGuidedLearningHours = fundingCsv.FundedGuidedLearningHours,
            };
        }
    }
}
