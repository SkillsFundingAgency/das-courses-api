using System.Collections.Generic;
using SFA.DAS.Courses.Domain.Courses;

namespace SFA.DAS.Courses.Domain.Interfaces
{
    public interface ILevelsService
    {
        IEnumerable<Level> GetAll();
    }
}