using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Entities.Versioning;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class ImportStandardsCommandHandler : IRequestHandler<ImportStandardsCommand, Unit>
    {
        private readonly IInstituteOfApprenticeshipService instituteOfApprenticeshipService;
        private readonly IStandardStagingRepository standardStagingRepository;

        public ImportStandardsCommandHandler(IInstituteOfApprenticeshipService instituteOfApprenticeshipService, IStandardStagingRepository standardStagingRepository)
        {
            this.instituteOfApprenticeshipService = instituteOfApprenticeshipService;
            this.standardStagingRepository = standardStagingRepository;
        }

        public async Task<Unit> Handle(ImportStandardsCommand request, CancellationToken cancellationToken)
        {
            var importedStandards = (await instituteOfApprenticeshipService.GetStandards<Domain.ImportTypes.Versioning.Standard>()).ToList();

            var stagedStandards = importedStandards.Select(s => (StandardStaging)s).ToList();

            standardStagingRepository.DeleteAll();

            await standardStagingRepository.InsertMany(stagedStandards);

            return Unit.Value;
        }
    }
}
