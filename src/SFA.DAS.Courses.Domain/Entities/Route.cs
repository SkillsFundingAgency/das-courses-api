namespace SFA.DAS.Courses.Domain.Entities
{
    public class Route : RouteBase
    {
        public static implicit operator Route(RouteImport source)
        {
            return new Route
            {
                Id = source.Id,
                Name = source.Name,
                Active = source.Active
            };
        }
    }
}
