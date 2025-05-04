using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class AdminService:Service<Admin>,IAdminService
    {
        private readonly IAdminRepository adminRepository;
        public AdminService(IAdminRepository adminRepository):base(adminRepository) 
        {
            this.adminRepository = adminRepository;
        }

        public AdminResponseDTO ConvertFromAdminToAdminResponseDTO(Admin admin)
        {

            
            AdminResponseDTO adminResponse = new AdminResponseDTO()
            {
                Email = admin.Email,
                Name = admin.Name
            };

            return adminResponse;
        }

        public List<AdminResponseDTO> ConvertFromAdminToAdminResponseDTOList(List<Admin> admins)
        {
            List<AdminResponseDTO> adminResponses = admins.Select(admin=>ConvertFromAdminToAdminResponseDTO(admin)).ToList();
            return adminResponses; 
        } 

        public Admin GetbyUserId(string UserId)
        {
            return adminRepository.GetByUserId(UserId); 
        }
    }
}
