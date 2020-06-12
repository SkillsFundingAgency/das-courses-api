namespace SFA.DAS.Courses.Functions.Importer.Domain.Configuration
{
    public class ImporterEnvironment
    {
        public virtual string EnvironmentName { get; }

        public ImporterEnvironment(string environmentName)
        {
            EnvironmentName = environmentName;
        }
    }
}