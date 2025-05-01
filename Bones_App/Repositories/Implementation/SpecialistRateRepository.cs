using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Implementation
{
    public class SpecialistRateRepository:Repository<SpecialistRate>,ISpecialistRateRepository
    {
        private readonly BonesContext context;
        public SpecialistRateRepository(BonesContext context):base(context)
        {
            this.context = context;
        }

        public SpecialistRate GetBySpecialistId(int specialistId)
        {
            SpecialistRate specialistRate = context.SpecialistRates.FirstOrDefault(rate =>  rate.SpecialistId == specialistId);
            return specialistRate;
        }

        public SpecialistRate GetPatientRatingForSpecialist(int specialistId, int patientId)
        {
            SpecialistRate specialistRate = context.SpecialistRates.FirstOrDefault(rate=>rate.SpecialistId == specialistId && rate.PatientId == patientId);
            return specialistRate;
        }

    }
}
