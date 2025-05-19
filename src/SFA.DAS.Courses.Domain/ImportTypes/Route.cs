namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class Route
    {
        public string Name { get; set; }

        public static implicit operator Route(string name)
        {
            return new Route
            {
                Name = name
            };
        }
    }
}
