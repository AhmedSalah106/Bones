using Bones_App.Commands.RatingCommand;
using Bones_App.DTOs;
using Bones_App.Helpers;
using MediatR;

namespace Bones_App.Handlers.CommandsHandler.RatingCommandHandler
{
    public class RateSpecialistCommandHandler : IRequestHandler<RateSpecialistCommand, SpecialistRateDTO>
    {
        private readonly IUnitOfWork unitOfWork;
        public RateSpecialistCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<SpecialistRateDTO> Handle(RateSpecialistCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();    
        }
    }
}
