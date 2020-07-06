using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Services
{
    public class LevelsService : ILevelsService
    {
        public IEnumerable<Level> GetAll()
        {
            return LevelsConstant.Levels;
        }
    }
}
