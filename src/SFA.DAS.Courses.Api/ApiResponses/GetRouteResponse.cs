using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetRouteResponse
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }

        public static implicit operator GetRouteResponse(Route source)
        {
            return new GetRouteResponse
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}