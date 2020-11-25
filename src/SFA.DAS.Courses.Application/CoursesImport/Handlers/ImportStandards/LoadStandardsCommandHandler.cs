using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Entities.Versioning;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.CoursesImport.Handlers.ImportStandards
{
    public class LoadStandardsCommandHandler : IRequestHandler<LoadStandardsCommand, Unit>
    {
        private readonly IStandardStagingRepository standardStagingRepository;
        private readonly IVersioningStandardRepository versioningStandardRepository;

        public LoadStandardsCommandHandler(IStandardStagingRepository standardStagingRepository, IVersioningStandardRepository versioningStandardRepository)
        {
            this.standardStagingRepository = standardStagingRepository;
            this.versioningStandardRepository = versioningStandardRepository;
        }

        public async Task<Unit> Handle(LoadStandardsCommand request, CancellationToken cancellationToken)
        {
            var allStandards = await standardStagingRepository.GetAll();

            await versioningStandardRepository.InsertMany(allStandards.Select(s => (Standard)s), allStandards.Select(s => (StandardAdditionalInformation)s));

            return Unit.Value;
        }
    }
}
