using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Data
{
    public static class KnowledgeBuilder
    {
        public static List<Knowledge> Create(params string[] knowledge) =>
            knowledge
                .Select(x => new Knowledge { KnowledgeId = Guid.NewGuid(), Detail = x })
                .ToList();
    }
}
