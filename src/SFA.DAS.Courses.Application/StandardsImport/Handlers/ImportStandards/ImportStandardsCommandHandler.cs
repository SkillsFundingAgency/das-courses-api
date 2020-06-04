using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.StandardsImport.Handlers.ImportStandards
{
    public class ImportStandardsCommandHandler : IRequestHandler<ImportStandardsCommand, Unit>
    {
        private readonly IStandardsImportService _standardsImportService;

        public ImportStandardsCommandHandler (IStandardsImportService standardsImportService)
        {
            _standardsImportService = standardsImportService;
        }
        public async Task<Unit> Handle(ImportStandardsCommand request, CancellationToken cancellationToken)
        {
            await _standardsImportService.ImportStandards();
            
            return Unit.Value;
        }
    }
}