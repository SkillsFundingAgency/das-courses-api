namespace SFA.DAS.Courses.Domain.Courses
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public static implicit operator Route(Entities.Route source)
        {
            if (source == null)
                return null;

            return new Route
            {
                Id = source.Id,
                Name = source.Name,
                Active = source.Active
            };
        }
    }
}
