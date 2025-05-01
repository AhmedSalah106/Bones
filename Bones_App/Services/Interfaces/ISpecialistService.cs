using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface ISpecialistService:IService<Specialist>
    {
        List<SpecialistResponseDTO> GetSpecialistDTOs(List<Specialist> specialists);
        List<SpecialistResponseDTO> GetAllDeletedSpecialists();
        SpecialistResponseDTO GetSpecialistDTO(Specialist specialist);
        SpecialistResponseDTO RestoreSpecialist(int Id);
        bool SoftDeleteSpecialist(int Id);
        Specialist GetByUserId(string UserId);
    }
}