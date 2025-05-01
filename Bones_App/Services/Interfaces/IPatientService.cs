using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IPatientService:IService<Patient>
    {


        List<Patient> GetAll(string Include);
        Patient GetById(int Id,string Include);
        List<PatientResponseDTO> GatAllPatientResponseDTOs();
        List<ImageResponseDTO> GetPatientImagesDTO(List<Image> Images);
        PatientResponseDTO GetPatientResponeDTO(Patient patient);
        bool SoftDeletePatient(int Id);
        List<PatientResponseDTO> GetAllDeletedPatientsDTO();
        PatientResponseDTO RestorePatient(int Id);
        Patient GetByUserId(string UserId);
    }
}