namespace SFA.DAS.Courses.Domain.Courses
{
    public class EqaProvider
    {
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string WebLink { get; set; }
        public static explicit operator EqaProvider(Entities.Standard source)
        {
            return new EqaProvider
            {
                Name = source.EqaProviderName,
                ContactName = source.EqaProviderContactName,
                ContactEmail = source.EqaProviderContactEmail,
                WebLink = source.EqaProviderWebLink
            };
        }
    }
}
