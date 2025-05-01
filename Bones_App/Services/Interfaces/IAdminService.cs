using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IAdminService:IService<Admin>
    {
        Admin GetbyUserId(string UserId);
        AdminResponseDTO ConvertFromAdminToAdminResponseDTO(Admin admin);
        List<AdminResponseDTO> ConvertFromAdminToAdminResponseDTOList(List<Admin> admins);
    }
}
