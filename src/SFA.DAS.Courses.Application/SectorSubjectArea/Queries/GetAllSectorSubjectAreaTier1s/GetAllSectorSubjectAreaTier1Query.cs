using System.Collections.Generic;
using MediatR;
using SFA.DAS.Courses.Domain.Entities;

namespace SFA.DAS.Courses.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

public record GetAllSectorSubjectAreaTier1Query : IRequest<List<SectorSubjectAreaTier1>>;
