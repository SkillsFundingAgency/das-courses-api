using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Courses.Domain.ImportTypes
{
    public class QualificationItemList
    {
        [JsonProperty("item-hash")]
        public List<string> ItemHash { get; set; }
    }
    
    public class QualificationItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent-qualification-sector-subject-area")]
        public string ParentQualificationSectorSubjectArea { get; set; }

        [JsonProperty("qualification-sector-subject-area")]
        public string QualificationSectorSubjectArea { get; set; }
    }
}