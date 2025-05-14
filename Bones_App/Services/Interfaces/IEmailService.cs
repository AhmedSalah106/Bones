using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IEmailService:IService<Emails>
    {
        Task<EmailResponseDTO> SendEmail(EmailDTO emailDto);
        List<Emails>GetEmailsBySpecialistId(string UserId);
        Emails GetEmails(EmailResponseDTO emailResponse);
    }
}
