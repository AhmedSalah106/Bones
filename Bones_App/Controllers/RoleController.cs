using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public RoleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AddRoleDto roleDto)
        {
            try
            {
                bool RoleExists = await unitOfWork.RoleManager.RoleExistsAsync(roleDto.RoleName);

                if (RoleExists)
                {
                    return BadRequest(new Response<string>("Role Already Exists"));
                }


                IdentityRole role = new IdentityRole(roleDto.RoleName);

                IdentityResult result = await unitOfWork.RoleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    return BadRequest(new Response<string>(result.Errors.ToString()));
                }

                return Ok(new Response<string>(roleDto.RoleName,$"Role {roleDto.RoleName} Successfully Added"));
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

        [HttpPost("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDTO roleDto)
        {
            try
            {
                ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync(roleDto.Email);
                if (user == null)
                {
                    return NotFound(new Response<string>("User Not Found"));
                }

                bool RoleExists = await unitOfWork.RoleManager.RoleExistsAsync(roleDto.RoleName);
                if (!RoleExists) 
                {
                    return NotFound(new Response<string>("Role Not Found"));
                }

                IdentityResult result = await unitOfWork.UserManager.AddToRoleAsync(user,roleDto.RoleName);
                if (!result.Succeeded)
                {
                    return BadRequest(new Response<string>(result.Errors.ToString()));
                }

                return Ok(new Response<string>(roleDto.RoleName,$"Role {roleDto.RoleName} Successfully Assigned to {user.UserName}"));
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
