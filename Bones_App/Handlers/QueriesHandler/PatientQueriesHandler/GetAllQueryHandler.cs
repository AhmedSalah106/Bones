using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Queries.PatientQuries;
using MediatR;

namespace Bones_App.Handlers.QueriesHandler.PatientQueriesHandler
{
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<PatientResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<PatientResponseDTO>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            return unitOfWork.PatientService.GatAllPatientResponseDTOs();
        }
    }
}
