using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Interfaces
{
    public interface ISpecialistRateRepository:IRepository<SpecialistRate>
    {
        SpecialistRate GetPatientRatingForSpecialist(int  specialistId , int patientId);
        SpecialistRate GetBySpecialistId(int specialistId);
    }
}
