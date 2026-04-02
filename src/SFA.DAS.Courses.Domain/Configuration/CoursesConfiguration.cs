namespace SFA.DAS.Courses.Domain.Configuration
{
    public class CoursesConfiguration
    {
        public string ConnectionString { get; set; }

        public SkillsEnglandApiConfiguration SkillsEnglandApiConfiguration { get; set; }
        public LarsImportConfiguration LarsImportConfiguration { get; set; }
    }

    public record SkillsEnglandApiConfiguration(string ApprenticeshipsApiUrl, string FoundationApprenticeshipsApiUrl, string ApprenticeshipUnitsApiUrl);
    public record LarsImportConfiguration(string LarsFundingCategory);
}
