using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Services.Implementation;
using Bones_App.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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


        [HttpGet("GetAllSpecialistUploadedImages")]
        public IActionResult GetAllSpecialistUploadedImages(string Id)
        {
            try
            {
                Response<List<ImageResponseDTO>> images = unitOfWork.ImageService.RetrieveImage(Id);
                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Details = ex.Message
                });
            }
        }

    }
}
