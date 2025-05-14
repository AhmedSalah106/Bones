using Bones_App.DTOs;
using Bones_App.Exceptions;
using Bones_App.Helpers;
using Bones_App.Migrations;
using Bones_App.Models;
using Bones_App.Repositories;
using Bones_App.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public AccountController
            (IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            
        }


        [HttpPost("/Account/Register")]
        public async Task<IActionResult> Register([FromForm]RegisterDTO userDTO)
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


                userDTO.Role = userDTO.Role.ToLower();
                if (userDTO.Role != "admin" && userDTO.Role != "patient" && userDTO.Role != "specialist")
                {
                    return BadRequest(new Response<string>("Enter valid Role"));
                }

                ApplicationUser user =await unitOfWork.UserManager.FindByNameAsync(userDTO.Email);

                if (user != null)
                {
                    return BadRequest(new Response<string>("This Email Already Exists"));
                }

                user = new ApplicationUser()
                {
                    UserName = userDTO.Email,
                    Email = userDTO.Email,
                    PasswordHash = userDTO.Password,
                    IsVerified = false,
                    FullName = userDTO.FullName
                };

                IdentityResult result = await unitOfWork.UserManager.CreateAsync(user, userDTO.Password);

                LoginResponseUserDataDTO userDataDTO = new LoginResponseUserDataDTO()
                {
                    Email = userDTO.Email,
                    UserName = userDTO.FullName,
                    PhoneNumber = userDTO.PhoneNumber
                };

                if (result.Succeeded == true)
                {
                    if (userDTO.Role == "patient")
                    {
                        user.IsVerified = true;
                        Patient patient = new Patient()
                        {
                            Name = userDTO.FullName,
                            Email = userDTO.Email,
                            User = user,
                            IsPaidUser = false,
                            FreeLimit = 2,
                            PhoneNumber = userDTO.PhoneNumber
                        };
                        
                        result =await unitOfWork.UserManager.AddToRoleAsync(user,"Patient");

                        if (!result.Succeeded)
                        {
                            return BadRequest(new Response<string>("Error happen while add role to this user"));
                        }
                        patient.User = user;
                        unitOfWork.PatientService.Insert(patient);
                        unitOfWork.Save();
                        userDataDTO.Id = patient.Id;

                    }
                    else if(userDTO.Role=="specialist")
                    {
                        Specialist specialist = new Specialist()
                        {
                            Name = userDTO.FullName,
                            Email = userDTO.Email,
                            PhoneNumber = userDTO.PhoneNumber,
                            User = user,
                            IsVerified = false
                        };

                        string wRootPathe = Path.Combine(unitOfWork.webHostEnvironment.WebRootPath, "Images");
                        string ImageName = Guid.NewGuid().ToString() + "_" + userDTO.CertificateFile.FileName;
                        string ImagePath = Path.Combine(wRootPathe, ImageName);
                        
                        using(FileStream fileStream = new FileStream(ImagePath,FileMode.Create))
                        {
                            userDTO.CertificateFile.CopyTo(fileStream);
                        }

                        specialist.CertificateUrl = Path.Combine("/Images", ImageName);

                        result = await unitOfWork.UserManager.AddToRoleAsync(user, "Specialist");

                        if(!result.Succeeded)
                        {
                            return BadRequest(new Response<string>("Error happen while add role to this user"));
                        }
                        specialist.User = user;
                        
                        unitOfWork.SpecialistService.Insert(specialist);
                        unitOfWork.Save();

                        userDataDTO.Id = specialist.Id;
                    }
                    else
                    {
                        return BadRequest(new Response<string>("Enter Valid Role"));
                    }



                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Falid To Create Account",
                        Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)
                    });
                }


                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email,userDTO.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                List<string> roles = (List<string>)await unitOfWork.UserManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(unitOfWork.configuration["JWT:SecritKey"]));
                SigningCredentials signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: unitOfWork.configuration["JWT:ValidISS"],
                    audience: unitOfWork.configuration["JWT:ValidAud"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: signing
                    );

                JWTResponseDTO jWTResponse = new JWTResponseDTO()
                {
                    Expire = jwt.ValidTo,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                    UserId = user.Id,
                    UserData = userDataDTO
                };

                return Ok(new Response<JWTResponseDTO>(jWTResponse, "Successfully Create Token"));

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

        [HttpPost("/Account/Login")]
        public async Task<IActionResult> Login(LoginDTO userDTO)
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
                LoginResponseUserDataDTO userDataDTO = new LoginResponseUserDataDTO();
                
                ApplicationUser user = await unitOfWork.UserManager.FindByNameAsync(userDTO.Email);
                if (user == null)
                {
                    return NotFound(new Response<string>("Invalid Data"));
                }
                if (userDTO.Role == "patient")
                {
                    Patient patient = unitOfWork.PatientService.GetByUserId(user.Id);
                    if (patient == null)
                    {
                        return NotFound(new Response<string>("Invalid Data"));
                    }
                    userDataDTO.Id = patient.Id;
                    userDataDTO.Email = userDTO.Email;
                    userDataDTO.PhoneNumber = patient.PhoneNumber;
                    userDataDTO.UserName = patient.Name;
                }
                else if (userDTO.Role == "specialist") 
                {
                    Specialist specialist = unitOfWork.SpecialistService.GetByUserId(user.Id);
                    if (specialist == null)
                    {
                        return NotFound(new Response<string>("Invalid Data"));
                    }
                    userDataDTO.Id = specialist.Id;
                    userDataDTO.Email = userDTO.Email;
                    userDataDTO.PhoneNumber = userDataDTO.PhoneNumber;
                    userDataDTO.UserName = specialist.Name;
                }
                else if(userDTO.Role =="admin")
                {
                    Admin admin = unitOfWork.AdminService.GetbyUserId(user.Id);
                    if (admin == null)
                    {
                        return NotFound(new Response<string>("Invalid Data"));
                    }
                    userDataDTO.Id = admin.Id;
                    userDataDTO.Email = admin.Email;
                    userDataDTO.PhoneNumber = null;
                    userDataDTO.UserName = admin.Name;
                }
                

                    bool Founded = await unitOfWork.UserManager.CheckPasswordAsync(user, userDTO.Password);

                if(!Founded)
                {
                    return NotFound(new Response<string>("Invalid Data"));
                }

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email,userDTO.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                List<string> roles = (List<string>)await unitOfWork.UserManager.GetRolesAsync(user);

                foreach(var role in  roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(unitOfWork.configuration["JWT:SecritKey"]));
                SigningCredentials signing = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: unitOfWork.configuration["JWT:ValidISS"],
                    audience: unitOfWork.configuration["JWT:ValidAud"],
                    claims:claims,
                    expires:DateTime.Now.AddHours(1),
                    signingCredentials: signing
                    );

                JWTResponseDTO jWTResponse = new JWTResponseDTO()
                {
                    Expire = jwt.ValidTo,
                    Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                    UserId = user.Id,
                    UserData = userDataDTO
                };


                return Ok(new Response<JWTResponseDTO>(jWTResponse,"Successfully Create Token"));

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

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequestDTO forgetPassword)
        {
            try
            {
                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(forgetPassword.Email);
                if(user == null)
                {
                    return NotFound(new Response<string>("No user found by this email"));
                }

                var code = new Random().Next(1000, 9999).ToString();
                user.PasswordResetCode = code;
                user.PasswordResetCodeExpiration = DateTime.UtcNow.AddMinutes(15);

                await unitOfWork.UserManager.UpdateAsync(user);
                EmailDTO email = new EmailDTO()
                {
                    DateSent = DateTime.UtcNow,
                    Body = $"Your password reset code is {code}",
                    Subject = "Reset Code",
                    To = forgetPassword.Email,
                    UserId = user.Id
                };

                 await unitOfWork.EmailService.SendEmail(email);


                return Ok(new Response<string>() { Success = true , Message = "Code Send Successfully"});


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

        [HttpPost("VerifyResetCode")]
        public async Task<IActionResult> VerifyResetCode(VerifyResetCodeRequestDTO resetCode)
        {
            try
            {
                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(resetCode.Email);

                if (user == null || user.PasswordResetCodeExpiration < DateTime.UtcNow || user.PasswordResetCode != resetCode.Code)
                {
                    return NotFound(new Response<string>("Invalid Or Expired Code"));
                }

                return Ok(new Response<string>() { Success = true, Message = "Ok" });
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

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDTO resetPassword)
        {
            try
            {
                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(resetPassword.Email);
                if (user == null || user.PasswordResetCode != resetPassword.Code || user.PasswordResetCodeExpiration < DateTime.UtcNow)
                {
                    return BadRequest(new Response<string>("Invalid or expired code."));
                }

                var resetToken = await unitOfWork.UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await unitOfWork.UserManager.ResetPasswordAsync(user, resetToken, resetPassword.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest(new Response<string>(result.Errors.ToString()));
                }

                user.PasswordResetCode = null;
                user.PasswordResetCodeExpiration = null;
                await unitOfWork.UserManager.UpdateAsync(user);

                return Ok(new Response<string>() { Success = true,Message = "Password Successfully Reset"});

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
