namespace SFA.DAS.Courses.Domain.Entities
{
    public class RouteImport : RouteBase
    {
        public static implicit operator RouteImport(string name)
        {
            return new RouteImport
            {
                Name = name
            };
        }

        public static implicit operator RouteImport(Route route)
        {
            return new RouteImport
            {
                Id = route.Id,
                Name = route.Name,
                Active = route.Active
            };
        }
    }
}
