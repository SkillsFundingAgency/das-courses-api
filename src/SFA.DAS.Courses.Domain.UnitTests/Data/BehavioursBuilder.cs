using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Courses.Domain.ImportTypes;

namespace SFA.DAS.Courses.Domain.UnitTests.Data
{
    public static class BehavioursBuilder
    {
        public static List<Behaviour> Create(params string[] knowledge) =>
            knowledge
                .Select(x => new Behaviour { BehaviourId = Guid.NewGuid().ToString(), Detail = x })
                .ToList();
    }
}
