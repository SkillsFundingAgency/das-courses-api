namespace SFA.DAS.Courses.Domain.Configuration
{
    public class CoursesConfiguration
    {
        public string ConnectionString { get; set; }

        public InstituteOfApprenticeshipsApiConfiguration InstituteOfApprenticeshipsApiConfiguration { get; set; }
    }

    public record InstituteOfApprenticeshipsApiConfiguration(string ApiBaseUrl, string StandardsPath, string FoundationApprenticeshipsPath);
}
