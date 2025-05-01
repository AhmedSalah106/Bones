using Bones_App.DTOs;
using System.Net.Mail;
using System.Net;
using Bones_App.Services.SharedService;
using Bones_App.Models;
using Bones_App.Services.Interfaces;
using Bones_App.Helpers;
using Bones_App.Repositories.Interfaces;

namespace Bones_App.Services.Implementation
{
    public class EmailService:Service<Emails> , IEmailService
    {
        private readonly IConfiguration configuration;
        private readonly IEmailRepository emailRepository;
        public EmailService(IConfiguration configuration , IEmailRepository emailRepository):base(emailRepository) 
        {
            this.emailRepository = emailRepository;
            this.configuration = configuration;
        }

        public List<Emails> GetEmailsBySpecialistId(int SpecialistId)
        {
            List<Emails> emails = emailRepository.GetAll().Where(email=>email.SpecialistId == SpecialistId).ToList();
            return emails;
        }

        public async Task<EmailResponseDTO> SendEmail(EmailDTO emailDto)
        {
            var fromEmail = configuration["EmailSettings:From"];
            var password = configuration["EmailSettings:Password"];
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailDto.To);

            await smtpClient.SendMailAsync(mailMessage);

            EmailResponseDTO responseDTO = new EmailResponseDTO()
            {
                DateSent = DateTime.UtcNow,
                Body = emailDto.Body,
                From = fromEmail,
                Subject = emailDto.Subject,
                SpecialistId = emailDto.SpecialistId
            };

            return responseDTO;
        }

        public Emails GetEmails(EmailResponseDTO emailResponse)
        {
            Emails email = new Emails()
            {
                DateSent = emailResponse.DateSent,
                Body = emailResponse.Body,
                From = emailResponse.From,
                Subject = emailResponse.Subject,
                SpecialistId = emailResponse.SpecialistId
            };
            return email;
        }
    }
}
