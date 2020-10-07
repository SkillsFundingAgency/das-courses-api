using System.Collections.Generic;

namespace SFA.DAS.Courses.Domain.Courses
{
    public static class LevelsConstant
    {
        public static readonly IEnumerable<Level> Levels = new List<Level>
        {
            new Level {Code = 2, Name = "GCSE"},
            new Level {Code = 3, Name = "A level"},
            new Level {Code = 4, Name = "higher national certificate (HNC)"},
            new Level {Code = 5, Name = "higher national diploma (HND)"},
            new Level {Code = 6, Name = "degree"},
            new Level {Code = 7, Name = "master’s degree"}
        };
    }
}
