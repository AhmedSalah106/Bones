using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface ISpecialistRateService:IService<SpecialistRate>
    {
        SpecialistRate ConvertToSpecialistRate(SpecialistRateDTO specialistRateDTO);
        SpecialistRateDTO ConvertToSpecialistRateDTO(SpecialistRate specialistRate);
        SpecialistRate GetPatientRatingForSpecialist(int SpecialistId, int PatientId);
        SpecialistRate GetBySpecialistId(int SpecialistId);
        int GetAverageRating(int SpecialistId);
        List<SpecialistRateResponseDTO> GetTopTenSpecialists();
    }
}
