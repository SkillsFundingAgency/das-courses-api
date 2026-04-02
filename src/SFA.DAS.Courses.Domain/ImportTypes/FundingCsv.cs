using System;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class FundingCsv
    {
        public string LearnAimRef { get; set; }
        public string FundingCategory { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal RateWeighted { get; set; }
        public decimal RateUnWeighted { get; set; }
        public string WeightingFactor { get; set; }
        public string AdultSkillsFundingBand { get; set; }
        public int? FundedGuidedLearningHours { get; set; }
    }
}
