using System;
using System.Collections.Generic;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FrameworkName { get; set; }
        public string PathwayName { get; set; }
        public int ProgType { get; set; }
        public int FrameworkCode { get; set; }
        public int PathwayCode { get; set; }
        public int Level { get; set; }
        public int TypicalLengthFrom { get; set; }
        public int TypicalLengthTo { get; set; }
        public string TypicalLengthUnit { get; set; }
        public int Duration { get; set; }
        public int CurrentFundingCap { get; set; }
        public int MaxFunding { get; set; }
        public double Ssa1 { get; set; }
        public double Ssa2 { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public bool IsActiveFramework { get; set; }
        public virtual ICollection<FrameworkFunding> FundingPeriods { get; set; }
        public int ProgrammeType { get; set; }
        public bool HasSubGroups { get; set; }
        public string ExtendedTitle { get; set; }
    }
}