namespace SFA.DAS.Courses.Domain.Entities
{
    public class Framework : FrameworkBase
    {
        public static implicit operator Framework(FrameworkImport frameworkImport)
        {
            return new Framework
            {
                Duration = frameworkImport.Duration,
                Id = frameworkImport.Id,
                Level = frameworkImport.Level,
                Ssa1 = frameworkImport.Ssa1,
                Ssa2 = frameworkImport.Ssa2,
                Title = frameworkImport.Title,
                EffectiveFrom = frameworkImport.EffectiveFrom,
                EffectiveTo = frameworkImport.EffectiveTo,
                ExtendedTitle = frameworkImport.ExtendedTitle,
                FrameworkCode = frameworkImport.FrameworkCode,
                FrameworkName = frameworkImport.FrameworkName,
                MaxFunding = frameworkImport.MaxFunding,
                PathwayCode = frameworkImport.PathwayCode,
                PathwayName = frameworkImport.PathwayName,
                ProgrammeType = frameworkImport.ProgrammeType,
                ProgType = frameworkImport.ProgType,
                CurrentFundingCap = frameworkImport.CurrentFundingCap,
                HasSubGroups = frameworkImport.HasSubGroups,
                IsActiveFramework = frameworkImport.IsActiveFramework,
                TypicalLengthFrom = frameworkImport.TypicalLengthFrom,
                TypicalLengthTo = frameworkImport.TypicalLengthTo,
                TypicalLengthUnit = frameworkImport.TypicalLengthUnit
            };
        }
    }
}