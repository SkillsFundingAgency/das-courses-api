using System;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class ApprenticeshipFundingCsv
    {
        public string ApprenticeshipType { get; set; }
        public int ApprenticeshipCode { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public decimal MaxEmployerLevyCap { get; set; }
    }
}