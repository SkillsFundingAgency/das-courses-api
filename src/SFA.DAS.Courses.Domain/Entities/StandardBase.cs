namespace SFA.DAS.Courses.Domain.Entities
{
    public class StandardBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public decimal Version { get; set; }
        public long MaxFunding { get; set; }
        public string OverviewOfRole { get; set; }
        public int TypicalDuration { get; set; }
        public string Route { get; set; }
        public string Keywords { get; set; }
        public string TypicalJobTitles { get; set; }
        public string CoreSkillsCount { get; set; }
        public string StandardPageUrl { get; set; }
        public string IntegratedDegree { get; set; }
        
    }
}