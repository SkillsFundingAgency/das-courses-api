using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Data.UnitTests.Customisations
{
    public class StandardRepositoryTestData
    {
        public List<Standard> ActiveValidApprenticeshipStandards { get; set; } = [];
        public List<Standard> ActiveInvalidApprenticeshipStandards { get; set; } = [];
        public List<Standard> NotYetApprovedApprenticeshipStandards { get; set; } = [];
        public List<Standard> WithdrawnApprenticeshipStandards { get; set; } = [];
        public List<Standard> RetiredApprenticeshipStandards { get; set; } = [];
        
        public List<Standard> ActiveValidFoundationApprenticeshipStandards { get; set; } = [];
        
        public List<Standard> ActiveValidShortCourseStandards { get; set; } = [];
        public List<Standard> ActiveInvalidShortCourseStandards { get; set; } = [];
        public List<Standard> NotYetApprovedShortCourseStandards { get; set; } = [];
        public List<Standard> WithdrawnShortCourseStandards { get; set; } = [];
        public List<Standard> RetiredShortCourseStandards { get; set; } = [];
        
        public List<Standard> AllStandards =>
        [
            .. ActiveValidApprenticeshipStandards,
            .. ActiveInvalidApprenticeshipStandards,
            .. NotYetApprovedApprenticeshipStandards,
            .. WithdrawnApprenticeshipStandards,
            .. RetiredApprenticeshipStandards,
            .. ActiveValidFoundationApprenticeshipStandards,
            .. ActiveValidShortCourseStandards,
            .. ActiveInvalidShortCourseStandards,
            .. NotYetApprovedShortCourseStandards,
            .. WithdrawnShortCourseStandards,
            .. RetiredShortCourseStandards
        ];
    }
}
