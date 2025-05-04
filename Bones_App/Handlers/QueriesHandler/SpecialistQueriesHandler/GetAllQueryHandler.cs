using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Queries.SpecialistQueries;
using MediatR;

namespace Bones_App.Handlers.QueriesHandler.SpecialistQueriesHandler
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<SpecialistResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<SpecialistResponseDTO>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return unitOfWork.SpecialistService.GetSpecialistDTOs(unitOfWork.SpecialistService.GetAll());
        }
    }
}
