using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Services.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly EmailService emailService;
        private readonly IUnitOfWork unitOfWork;
        public EmailController(EmailService emailService,
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.emailService = emailService;
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailDTO emailDto)
        {
            try
            {
                EmailResponseDTO emailResponse = await emailService.SendEmail(emailDto);

                Emails email= unitOfWork.EmailService.GetEmails(emailResponse);
                email.UserId = emailDto.UserId;
                
                unitOfWork.EmailService.Insert(email);
                unitOfWork.Save();

                return Ok(new Response<EmailResponseDTO>(emailResponse,"Email Sent Successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllEmails()
        {
            try
            {
                List<Emails> emails = unitOfWork.EmailService.GetAll();

                return Ok(new Response<List<Emails>>(emails, "Successfully Retrieved"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Details = ex.Message
                });
            }

        }

        [HttpGet("GetAllSpecialistEmails")]
        public IActionResult GetAllSpecialistEmails(string SpecialistID)
        {
            try
            {
                List<Emails> emails = unitOfWork.EmailService.GetEmailsBySpecialistId(SpecialistID);
                if(emails==null||emails.Count==0)
                {
                    return BadRequest(new Response<string>("No Emails have been sent Yet"));
                }

                return Ok(new Response<List<Emails>>(emails,"Emails Successfully retrieved"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Details = ex.Message
                });
            }
        }

    }
}
