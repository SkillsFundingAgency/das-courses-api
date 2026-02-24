using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportDataCommand: IRequest<List<string>>
    {
    }
}
