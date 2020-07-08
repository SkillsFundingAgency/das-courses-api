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

        public ImportStandardsCommandHandler (IStandardsImportService standardsImportService, ILarsImportService larsImportService)
        {
            _standardsImportService = standardsImportService;
            _larsImportService = larsImportService;
        }
        public async Task<Unit> Handle(ImportStandardsCommand request, CancellationToken cancellationToken)
        {
            var standardsImport =  _standardsImportService.ImportStandards();
            var larsImportResult =  _larsImportService.ImportData();
            
            await Task.WhenAll(larsImportResult, standardsImport);
            
            return Unit.Value;
        }
    }
}