using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Implementation;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class PatientService: Service<Patient>, IPatientService
    {
        private readonly IPatientRepository repository;
        public PatientService(IPatientRepository repository):base(repository)
        {
            this.repository = repository;
        }

        public List<PatientResponseDTO> GatAllPatientResponseDTOs()
        {
            List<PatientResponseDTO> patientResponseDTOs = 
                GetAll().Where(x=>x.IsDeletedUser==false).Select(patient=>new PatientResponseDTO 
                { Id = patient.Id, Name = patient.Name, Email = patient.Email
                    ,FreeLimit=patient.FreeLimit }).ToList();

            return patientResponseDTOs;
        }

        public List<Patient> GetAll(string Include)
        {
            return repository.GetAll(Include).Where(p=>p.IsDeletedUser = false).ToList();
        }

        public Patient GetById(int Id, string Include)
        {
            return repository.GetById(Id, Include);
        }

        public List<ImageResponseDTO> GetPatientImagesDTO(List<Image> Images)
        {
            List<ImageResponseDTO> retrieveImageDTOs = 
                Images.Select(img => new ImageResponseDTO 
                        { Id = img.Id, ImageUrl = img.ImageURL, UploadedAt = img.UploadedAt }).ToList();

            return retrieveImageDTOs;
        }

        public PatientResponseDTO GetPatientResponeDTO(Patient patient)
        {
            PatientResponseDTO patientResponse = new PatientResponseDTO()
            {
                UserId = patient.UserId,
                Id = patient.Id,
                Name = patient.Name,
                Email = patient.Email,
                FreeLimit = patient.FreeLimit
            };

            return patientResponse;
        }

        public PatientResponseDTO RestorePatient(int Id)
        {
            Patient patient = repository.GetById(Id);
            if(patient==null)
            {
                return null;
            }

            patient.IsDeletedUser = false;

            repository.Update(patient);
            return GetPatientResponeDTO(patient);
        }

        public bool SoftDeletePatient(int Id)
        {
            Patient patient = repository.GetById(Id);
            if(patient == null)
            {
                return false;
            }
            patient.IsDeletedUser = true;
            return true;
        }


        public List<PatientResponseDTO>GetAllDeletedPatientsDTO()
        {

            List<PatientResponseDTO> patientsResponse = 
                GetAll().Where(x=>x.IsDeletedUser==true).Select(x => new PatientResponseDTO()
                {UserId=x.UserId,Email=x.Email,FreeLimit=x.FreeLimit,Id=x.Id,Name=x.Name }).ToList();

            return patientsResponse;
        }

        public Patient GetByUserId(string UserId)
        {
            Patient patient= repository.GetByUserId(UserId);
            Patient patient1 = patient;
            return patient;
        }
    }
}
