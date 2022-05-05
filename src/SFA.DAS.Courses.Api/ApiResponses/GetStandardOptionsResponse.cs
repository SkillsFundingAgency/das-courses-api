namespace SFA.DAS.Courses.Api.ApiResponses
{
    public class GetStandardOptionsResponse
    {
        public StandardOptionKsb[] KSBs { get; set; }
    }

    public class StandardOptionKsb
    {
        public KsbType Type { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }
    }

    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
    }
}

