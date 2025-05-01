using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Services.Implementation;
using Bones_App.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialistController : ControllerBase
    {

        private readonly IUnitOfWork unitOfWork;
        public SpecialistController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<SpecialistResponseDTO> specialists =
                    unitOfWork.SpecialistService.GetSpecialistDTOs(unitOfWork.SpecialistService.GetAll());

                if (specialists == null || specialists.Count() == 0)
                {
                    return NotFound("No Specialists Founded Currently");
                }

                return Ok(specialists);
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
