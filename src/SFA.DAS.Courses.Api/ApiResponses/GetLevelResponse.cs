using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetLevelResponse
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public static implicit operator GetLevelResponse(Level source)
        {
            return new GetLevelResponse
            {
                Code = source.Code,
                Name = source.Name
            };
        }
    }
}
