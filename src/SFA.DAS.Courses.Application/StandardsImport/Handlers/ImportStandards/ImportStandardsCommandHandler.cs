using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards
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
            var larsImportResult = _larsImportService.ImportData();
            var standardsImport =  _standardsImportService.ImportStandards();

            await Task.WhenAll(larsImportResult, standardsImport);
            
            return Unit.Value;
        }
    }
}