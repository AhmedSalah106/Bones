using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Services.Implementation;
using Bones_App.Services.Jobs;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Formats.Asn1;
using System.Threading.Tasks;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class AdminController : ControllerBase
    {

        private readonly IUnitOfWork unitOfWork;
        public AdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        [HttpGet("GetAllUsersId")]
        public IActionResult GetAllUsersId()
        {
            try
            {
                List<ApplicationUser> Users = unitOfWork.UserManager.Users.ToList();

                return Ok(new Response<List<ApplicationUser>>(Users));
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

        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string UserEmail)
        {
            try
            {
                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(UserEmail);
                if(user==null)
                {
                    return NotFound(new Response<string>("No User Register by this email"));
                }

                List<string> roles = (List<string>)await unitOfWork.UserManager.GetRolesAsync(user);

                return Ok(new Response<List<string>>(roles,"All roles successfully retrieved"));
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

        [HttpGet("GetAllAdmins")]
        public IActionResult GetAllAdmins()
        {
            try
            {
                List<Admin> admins = unitOfWork.AdminService.GetAll();
                if (admins == null || admins.Count == 0)
                {
                    return NotFound(new Response<string>("No Admins Added Yet"));
                }

                List<AdminResponseDTO> adminResponses = unitOfWork.AdminService.ConvertFromAdminToAdminResponseDTOList(admins);

                return Ok(new Response<List<AdminResponseDTO>>(adminResponses, "All Admins Successfully Retrieved"));
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
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin(AdminRequestDTO adminDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        Message = "Invalid Input Data",
                        Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
                    });
                }
                bool founded = await unitOfWork.RoleManager.RoleExistsAsync("Admin");
                if (!founded)
                {
                    return NotFound(new Response<string>("Role Not Found"));
                }

                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(adminDTO.Email);
                if (user != null)
                {
                    return BadRequest(new Response<string>("This Email Already Found"));
                }

                user = new ApplicationUser()
                {
                    UserName = adminDTO.Email,
                    Email = adminDTO.Email,
                    PasswordHash = adminDTO.Password
                };

                IdentityResult result = await unitOfWork.UserManager.CreateAsync(user, adminDTO.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(new Response<string>("Invalid Data"));
                }


                result = await unitOfWork.UserManager.AddToRoleAsync(user, "Admin");
                if (!result.Succeeded)
                {
                    return BadRequest(new Response<string>("Invalid Data"));
                }

                Admin admin = new Admin()
                {
                    Email = adminDTO.Email,
                    Name = adminDTO.Name,
                    User = user,
                    UserId = user.Id,
                    Password = adminDTO.Password
                };

                unitOfWork.AdminService.Insert(admin);
                unitOfWork.Save();

                AdminResponseDTO adminResponse = unitOfWork.AdminService.ConvertFromAdminToAdminResponseDTO(admin);

                return Ok(new Response<AdminResponseDTO>(adminResponse, "Admin Successfully Added"));

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


        [HttpGet("GetAllDeletedSpecialists")]
        public IActionResult GetAllDeletedSpecialists()
        {
            try
            {
                List<SpecialistResponseDTO> AllDeletedSpecialists = unitOfWork.SpecialistService.GetAllDeletedSpecialists();

                if (AllDeletedSpecialists == null || AllDeletedSpecialists.Count == 0)
                {
                    return NotFound(new Response<string>("No Deleted Specialist"));  
                }

                return Ok(new Response<List<SpecialistResponseDTO>>(AllDeletedSpecialists,"All Deleted Specialists Retrieved Successfully"));
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


        [HttpGet("GetAllDeletedPatients")]
        public IActionResult GetAllDeletedPatients()
        {
            try
            {
                List<PatientResponseDTO> AllDeletedPatientss = unitOfWork.PatientService.GetAllDeletedPatientsDTO();

                if (AllDeletedPatientss == null || AllDeletedPatientss.Count == 0)
                {
                    return NotFound(new Response<string>("No Deleted Patients"));
                }

                return Ok(new Response<List<PatientResponseDTO>>(AllDeletedPatientss, "All Deleted Patients Retrieved Successfully"));
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


        [HttpPost("DeleteSpecialist")]
        public IActionResult DeleteSpecialist(int Id)
        {
            try
            {
                
                bool IsDeleted = unitOfWork.SpecialistService.SoftDeleteSpecialist(Id);

                if (!IsDeleted)
                {
                    return NotFound(new Response<string>("No Specialist by this Id"));
                }

                unitOfWork.Save();

                BackgroundJob.Schedule<SpecialistJobs>(s => s.DeleteSpecialist(Id), TimeSpan.FromDays(1));


                return Ok(new Response<string>("","Specialist Deleted Successfully"));

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

        [HttpPost("DeletePatient")]
        public IActionResult DeletePatient(int Id)
        {
            try
            {
                bool IsDeleted = unitOfWork.PatientService.SoftDeletePatient(Id);

                if (IsDeleted == false)
                {
                    return NotFound(new Response<string>("No Specialist Founded by this id"));
                }

                unitOfWork.Save();

                BackgroundJob.Schedule<PatientJobs>(s => s.DeletePatient(Id), TimeSpan.FromDays(1));


                return Ok(new Response<string>("", "Patient Deleted Successfully"));

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

        [HttpPost("ResotrePatient")]
        public IActionResult RestorePatient(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new Response<string>(ModelState.ToString()));
                }

                PatientResponseDTO patientResponse = unitOfWork.PatientService.RestorePatient(id);
                if (patientResponse == null)
                {
                    return NotFound(new Response<string>("No Specialist founded by this Id"));
                }

                unitOfWork.Save();

                return Ok(new Response<PatientResponseDTO>(patientResponse, "Specialist Successfully Restored"));
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


        [HttpPost("ResotreSpecialist")]
        public IActionResult RestoreSpecialist(int id)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(new Response<string>(ModelState.ToString()));
                }

                SpecialistResponseDTO specialistResponse = unitOfWork.SpecialistService.RestoreSpecialist(id);
                if(specialistResponse==null)
                {
                    return NotFound(new Response<string>("No Specialist founded by this Id"));
                }

                unitOfWork.Save();
                return Ok(new Response<SpecialistResponseDTO>(specialistResponse,"Specialist Successfully Restored"));

                
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

        [HttpPost("AcceptSpecialist")]
        public async Task<IActionResult> AcceptSpecialist(int Id)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return NotFound(new Response<string>(ModelState.ToString()));
                }

                Specialist specialist = unitOfWork.SpecialistService.GetById(Id);

                var user =await unitOfWork.UserManager.FindByIdAsync(specialist.UserId);

                if(user==null)
                {
                    return BadRequest(new Response<string>("Not Valid Data"));
                }

                user.IsVerified = true;

                var resutl = await unitOfWork.UserManager.UpdateAsync(user);

                if(!resutl.Succeeded)
                {
                    return BadRequest(new Response<string>("Not Valid Data"));
                }

                EmailDTO email = new EmailDTO()
                {
                    Subject = $"Dear {specialist.Name}",
                    Body = $@"You Have Been Accepted in our App Bones_App" ,
                    To = specialist.Email,
                    DateSent = DateTime.UtcNow,
                    SpecialistId = Id,
                };

                BackgroundJob.Enqueue<EmailJobs>(Email => Email.SendEmail(email));
                SpecialistResponseDTO specialistResponse = unitOfWork.SpecialistService.GetSpecialistDTO(specialist);
                unitOfWork.Save();

                return Ok(new Response<SpecialistResponseDTO>(specialistResponse,"This Specialist have Been Accepted"));
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
        [HttpGet("GetTotalPayments")]
        public IActionResult GetTotalPayments()
        {
            try
            {
                decimal TotalPayments = unitOfWork.PaymentTransactionService.GetTotalTransactionPayments();
                if (TotalPayments == null || TotalPayments == 0)
                {
                    return NotFound(new Response<string>("No Transactions Yet "));
                }
                return Ok(new Response<decimal>(TotalPayments,"Total Transaction payments Successfully Retrieved"));
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


        [HttpGet("GetAllTransactions")]
        public IActionResult GetAllTransaction()
        {
            try
            {
                List<PaymentTransactionResponseDTO> paymentTransactionResponses = 
                    unitOfWork.PaymentTransactionService
                    .ConvertFromPaymentTransactionToPaymentTransactionResponseDTOList
                        (unitOfWork.PaymentTransactionService.GetAll());

                if(paymentTransactionResponses==null|| paymentTransactionResponses.Count == 0)
                {
                    return NotFound(new Response<string>("No Transactions Yet"));
                }

                return Ok(new Response<List<PaymentTransactionResponseDTO>>(paymentTransactionResponses,"All Transactions Successfully Retrieved"));
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
