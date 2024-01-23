using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Entities;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

public class GetAllSectorSubjectAreaTier1QueryHandler : IRequestHandler<GetAllSectorSubjectAreaTier1Query, List<SectorSubjectAreaTier1>>
{
    private readonly ISectorSubjectAreaTier1Repository _sectorSubjectAreaTier1Repository;

    public GetAllSectorSubjectAreaTier1QueryHandler(ISectorSubjectAreaTier1Repository sectorSubjectAreaTier1Repository)
    {
        _sectorSubjectAreaTier1Repository = sectorSubjectAreaTier1Repository;
    }

    public async Task<List<SectorSubjectAreaTier1>> Handle(GetAllSectorSubjectAreaTier1Query request, CancellationToken cancellationToken)
       => await _sectorSubjectAreaTier1Repository.GetAll(cancellationToken);
}
