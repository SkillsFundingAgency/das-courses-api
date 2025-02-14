namespace SFA.DAS.Courses.Application.CoursesImport.Extensions.StringExtensions
{
    public static class StringExtensions
    {
        public static (int Major, int Minor) ParseVersion(this string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return (0, 0);
            }

            var versionParts = version.Split('.');
            var major = int.TryParse(versionParts[0], out var parsedMajor) ? parsedMajor : 0;
            var minor = versionParts.Length > 1 && int.TryParse(versionParts[1], out var parsedMinor) ? parsedMinor : 0;

            return (major, minor);
        }
    }

}
