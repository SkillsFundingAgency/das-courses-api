using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Courses
{
    public static class LevelsConstant
    {
        public static readonly IEnumerable<Level> Levels = new List<Level>
        {
            new Level {Code = 2, Name = "GCSE grades 9 to 4"},
            new Level {Code = 3, Name = "A level"},
            new Level {Code = 4, Name = "HNC"},
            new Level {Code = 5, Name = "foundation degree"},
            new Level {Code = 6, Name = "degree"},
            new Level {Code = 7, Name = "master’s degree"}
        };
    }
}
