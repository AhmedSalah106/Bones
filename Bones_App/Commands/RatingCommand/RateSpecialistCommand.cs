using Bones_App.DTOs;
using MediatR;

namespace Bones_App.Commands.RatingCommand
{
    public class RateSpecialistCommand:IRequest<SpecialistRateDTO>
    {
        public SpecialistRateDTO specialistRate {  get; set; }
        public RateSpecialistCommand(SpecialistRateDTO specialistRate)
        {
            this.specialistRate = specialistRate;
        }
    }
}
