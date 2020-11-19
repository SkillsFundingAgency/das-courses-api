using MediatR;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportStandardsCommand : IRequest<Unit> { }

    public class ImportStandardDocumentsCommand : IRequest<Unit> { }
}
