using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace SFA.DAS.Courses.Domain.ImportTypes.Versioning
{
    public class Standard
    {
        public int LarsCode { get; set; }

        public string ReferenceNumber { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string Version { get; set; }

        public DateTime? EarlierStartDate { get; set; }

        public DateTime? LatestStartDate { get; set; }

        public DateTime? LatestEndDate { get; set; }

        public string OverviewOfRole { get; set; }

        public int Level { get; set; }

        public int TypicalDuration { get; set; }

        public decimal MaxFunding { get; set; }

        public string Route { get; set; }

        public List<string> Keywords { get; set; } = new List<string>();

        public List<string> JobRoles { get; set; } = new List<string>();

        public string SSA1 { get; set; }

        public string SSA2 { get; set; }

        public string StandardInformation { get; set; }

        public List<Knowledge> Knowledges { get; set; } = new List<Knowledge>();

        public List<Behaviour> Behaviours { get; set; } = new List<Behaviour>();

        public List<Skill> Skills { get; set; } = new List<Skill>();

        public List<Option> Options { get; set; } = new List<Option>();

        public List<string> OptionsUnstructuredTemplate { get; set; } = new List<string>();

        public string IntegratedDegree { get; set; }

        public List<string> TypicalJobTitles { get; set; } = new List<string>();

        public Uri? StandardPageUrl { get; set; }

        public List<Duty> Duties { get; set; } = new List<Duty>();

        public bool? CoreAndOptions { get; set; }

        public bool? IntegratedApprenticeship { get; set; }
    }

    public class Option
    {
        public string OptionId { get; set; }
        public string Title { get; set; }
    }

    public class Skill
    {
        public string SkillId { get; set; }
        public string Detail { get; set; }
    }

    public class Duty
    {
        public Guid DutyId { get; set; }

        public string DutyDetail { get; set; }

        public long IsThisACoreDuty { get; set; }

        public List<Guid> MappedBehaviour { get; set; }

        public List<Guid> MappedKnowledge { get; set; }

        public object MappedOptions { get; set; }

        public List<Guid> MappedSkills { get; set; }

        public string CriteriaForMeasuringPerformance { get; set; }
    }

    public class Behaviour
    {
        public string BehaviourId { get; set; }

        public string Detail { get; set; }
    }

    public class Knowledge
    {
        public string KnowledgeId { get; set; }

        public string Detail { get; set; }
    }
}
