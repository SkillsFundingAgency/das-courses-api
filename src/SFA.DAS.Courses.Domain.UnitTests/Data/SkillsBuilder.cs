using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Data
{
    public static class SkillsBuilder
    {
        public static List<Skill> Create(params string[] knowledge) =>
            knowledge
                .Select(x => new Skill { SkillId = Guid.NewGuid(), Detail = x })
                .ToList();
    }
}
