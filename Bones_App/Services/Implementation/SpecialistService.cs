using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class SpecialistService: Service<Specialist>, ISpecialistService
    {
        private readonly ISpecialistReposiotry reposiotry;
        public SpecialistService(ISpecialistReposiotry reposiotry):base(reposiotry) 
        {
            this.reposiotry = reposiotry;
        }

        public List<SpecialistResponseDTO> GetAllDeletedSpecialists()
        {
            List<SpecialistResponseDTO> AllDeletedSpecialists = GetAll()
                .Where(specialist=>specialist.IsDeleted==true)
                    .Select(specialist=>new SpecialistResponseDTO 
                        {Id=specialist.Id,Email=specialist.Email ,UserId=specialist.UserId, Name=specialist.Name, PhoneNumber=specialist.PhoneNumber })
                            .ToList();

            return AllDeletedSpecialists;

        }

        public List<SpecialistResponseDTO> GetSpecialistDTOs(List<Specialist> specialists)
        {
            List<SpecialistResponseDTO> specialistResponseDTOs = 
                specialists.Where(specialists=>specialists.IsDeleted==false).Select(specialist => new SpecialistResponseDTO 
                { Id = specialist.Id, Name = specialist.Name, Email = specialist.Email 
                    ,UserId=specialist.UserId,PhoneNumber = specialist.PhoneNumber}).ToList();

            return specialistResponseDTOs;
        }

        public SpecialistResponseDTO GetSpecialistDTO(Specialist specialist)
        {
            SpecialistResponseDTO specialistResponse = new SpecialistResponseDTO()
            {
                UserId = specialist.UserId,
                Id = specialist.Id,
                Email = specialist.Email,
                Name = specialist.Name,
                PhoneNumber = specialist.PhoneNumber
            };
            return specialistResponse;
        }

        public bool SoftDeleteSpecialist(int Id)
        {
            Specialist specialist = reposiotry.GetById(Id);
            if (specialist == null)
            {
                return false;
            }

            specialist.IsDeleted = true;

            reposiotry.Update(specialist);

            return true;
        }

        public SpecialistResponseDTO RestoreSpecialist(int Id)
        {
            Specialist specialist = reposiotry.GetById(Id);
            if(specialist == null)
            {
                return null;
            }

            specialist.IsDeleted = false;

            reposiotry.Update(specialist);

            return GetSpecialistDTO(specialist);
        }

        public Specialist GetByUserId(string UserId)
        {
            return reposiotry.GetByUserId(UserId);
        }
    }
}
