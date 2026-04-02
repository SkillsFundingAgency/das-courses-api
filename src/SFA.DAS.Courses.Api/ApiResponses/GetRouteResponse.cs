using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetRouteResponse
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public bool Active { get; set ; }

        public static implicit operator GetRouteResponse(Route source)
        {
            if(source == null) 
                return null;

            return new GetRouteResponse
            {
                Id = source.Id,
                Name = source.Name,
                Active = source.Active
            };
        }
    }
}
