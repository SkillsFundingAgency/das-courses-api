using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Courses.Domain.Courses;
using SFA.DAS.Courses.Domain.Interfaces;

namespace SFA.DAS.Courses.Application.Courses.Queries.GetSectors
{
    public class GetSectorsListQueryHandler : IRequestHandler<GetSectorsListQuery, GetSectorsListResult>
    {
        private readonly ISectorService _sectorService;

        public GetSectorsListQueryHandler (ISectorService sectorService)
        {
            _sectorService = sectorService;
        }
        public async Task<GetSectorsListResult> Handle(GetSectorsListQuery request, CancellationToken cancellationToken)
        {
            var sectors = await _sectorService.GetSectors();
            
            return new GetSectorsListResult
            {
                Sectors = sectors
            }; 
                
        }
    }
}