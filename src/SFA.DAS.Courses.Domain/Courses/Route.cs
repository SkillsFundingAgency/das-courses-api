namespace SFA.DAS.Courses.Domain.Courses
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Route(Domain.Entities.Route route)
        {
            return new Route
            {
                Id = route.Id,
                Name = route.Name
            };
        }
    }
}