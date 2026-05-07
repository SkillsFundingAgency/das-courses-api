using System.Collections.Generic;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportDataCommandResult
    {
        public List<string> ValidationMessages { get; set; } = new();
        public bool StandardsLoadedSuccessfully { get; set; }
    }
}

