namespace SFA.DAS.Courses.Domain.Entities
{
    public class FrameworkImport : FrameworkBase
    {
        public static implicit operator FrameworkImport(ImportTypes.Framework framework)
        {
            return new FrameworkImport
            {
                Id = framework.Id,
                Duration = framework.Duration,
                Level = framework.Level,
                Ssa1 = framework.Ssa1,
                Ssa2 = framework.Ssa2,
                Title = framework.Title,
                EffectiveFrom = framework.EffectiveFrom,
                EffectiveTo = framework.EffectiveTo,
                ExtendedTitle = framework.ExtendedTitle,
                FrameworkCode = framework.FrameworkCode,
                FrameworkName = framework.FrameworkName,
                MaxFunding = framework.MaxFunding,
                PathwayCode = framework.PathwayCode,
                PathwayName = framework.PathwayName,
                ProgrammeType = framework.ProgrammeType,
                ProgType = framework.ProgType,
                CurrentFundingCap = framework.CurrentFundingCap,
                HasSubGroups = framework.HasSubGroups,
                IsActiveFramework = framework.IsActiveFramework,
                TypicalLengthFrom = framework.TypicalLength.From,
                TypicalLengthTo = framework.TypicalLength.To,
                TypicalLengthUnit = framework.TypicalLength.Unit
            };
        }
    }
}