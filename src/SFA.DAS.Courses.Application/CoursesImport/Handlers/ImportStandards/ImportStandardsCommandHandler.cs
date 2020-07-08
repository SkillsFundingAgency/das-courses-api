using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportStandardsCommandHandler : IRequestHandler<ImportStandardsCommand, Unit>
    {
        private readonly IStandardsImportService _standardsImportService;
        private readonly ILarsImportService _larsImportService;
        private readonly IFrameworksImportService _frameworksImportService;

        public ImportStandardsCommandHandler (IStandardsImportService standardsImportService, ILarsImportService larsImportService, IFrameworksImportService frameworksImportService)
        {
            _standardsImportService = standardsImportService;
            _larsImportService = larsImportService;
            _frameworksImportService = frameworksImportService;
        }
        public async Task<Unit> Handle(ImportStandardsCommand request, CancellationToken cancellationToken)
        {
            var standardsImport =  _standardsImportService.ImportStandards();
            var larsImportResult =  _larsImportService.ImportData();
            var frameworksResult = _frameworksImportService.ImportData();
            
            await Task.WhenAll(larsImportResult, standardsImport, frameworksResult);
            
            return Unit.Value;
        }
    }
}