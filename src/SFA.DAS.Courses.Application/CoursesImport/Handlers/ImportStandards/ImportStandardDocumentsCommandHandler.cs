using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportStandardDocumentsCommandHandler : IRequestHandler<ImportStandardDocumentsCommand, Unit>
    {
        private readonly IInstituteOfApprenticeshipService instituteOfApprenticeshipService;
        private readonly IStandardDocumentRepository standardDocumentRepository;

        public ImportStandardDocumentsCommandHandler(IInstituteOfApprenticeshipService instituteOfApprenticeshipService, IStandardDocumentRepository standardDocumentRepository)
        {
            this.instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            this.standardDocumentRepository = standardDocumentRepository;
        }

        public async Task<Unit> Handle(ImportStandardDocumentsCommand request, CancellationToken cancellationToken)
        {
            var importedStandards = await instituteOfApprenticeshipService.GetStandardDocuments();
            var keys = importedStandards.Select(x => standardDocumentRepository.SaveDocument(x.Key, x.Value)).ToList();
            return Unit.Value;
        }
    }
}
