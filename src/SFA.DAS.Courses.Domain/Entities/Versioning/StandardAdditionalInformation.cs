using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Courses.Domain.Entities.Versioning
{
    public class StandardAdditionalInformation
    {
        public string StandardUId { get; set; }
        public string StandardInformation { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Knowledges { get; set; }
        public List<string> Behaviours { get; set; }
        public List<string> Skills { get; set; }
        public List<string> Options { get; set; }

        public static implicit operator StandardAdditionalInformation(StandardStaging importedStandard) => new StandardAdditionalInformation
        {
            StandardUId = importedStandard.StandardUId,
            StandardInformation = importedStandard.StandardInformation,
            Keywords = importedStandard.Keywords,
            Knowledges = importedStandard.Knowledges,
            Behaviours = importedStandard.Behaviours,
            Skills = importedStandard.Skills,
            Options = importedStandard.Options.Any() ? importedStandard.Options : importedStandard.OptionsUnstructuredTemplate.Any() ? importedStandard.OptionsUnstructuredTemplate : new List<string>()
        };
    }
}
