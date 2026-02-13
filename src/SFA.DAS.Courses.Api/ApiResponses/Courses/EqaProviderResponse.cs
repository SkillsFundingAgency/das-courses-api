using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class EqaProviderResponse
    {
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string WebLink { get; set; }
        public static explicit operator EqaProviderResponse(EqaProvider source)
        {
            return new EqaProviderResponse
            {
                Name = source.Name,
                ContactName = source.ContactName,
                ContactEmail = source.ContactEmail,
                WebLink = source.WebLink
            };
        }
    }
}

