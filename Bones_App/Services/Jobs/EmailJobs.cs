using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Services.Implementation;
using Bones_App.Services.Interfaces;
using System.Threading.Tasks;

namespace Bones_App.Services.Jobs
{
    public class EmailJobs
    {
        private readonly EmailService emailService;
        private readonly IUnitOfWork unitOfWork;
        public EmailJobs(IUnitOfWork unitOfWork,
            EmailService emailService)
        {
            this.unitOfWork = unitOfWork; 
            this.emailService = emailService;
        }
        public async Task SendEmail(EmailDTO emailDTO)
        {
            EmailResponseDTO emailResponse = await emailService.SendEmail(emailDTO);
            Emails email = emailService.GetEmails(emailResponse);

            email.UserId = email.UserId;

            emailService.Insert(email);
            unitOfWork.Save();
        }
    }
}
