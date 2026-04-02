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

        public static implicit operator RouteImport(Route source)
        {
            if (source == null)
                return null;

            return new RouteImport
            {
                Id = source.Id,
                Name = source.Name,
                Active = source.Active
            };
        }
    }
}
