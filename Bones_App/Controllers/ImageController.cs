using Bones_App.DTOs;
using Bones_App.Exceptions;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly IMediator mediator;
        public ImageController(IMediator mediator,IUnitOfWork unitOfWork
            , IWebHostEnvironment webHostEnvironment)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        

        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromForm] UploadImageDTO imageDTO) 
        {
            try
            {

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                string wRootPathe = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                string ImageName = Guid.NewGuid().ToString() + "_" + imageDTO.ImageFile.FileName;
                string ImagePath = Path.Combine(wRootPathe, ImageName);

                using (FileStream fileStream = new FileStream(ImagePath, FileMode.Create))
                {
                    imageDTO.ImageFile.CopyTo(fileStream);
                }

                Image image = new Image()
                {
                    ImageURL = Path.Combine("/Images",ImageName),
                    PatientID = imageDTO.Id,
                    UploadedAt = imageDTO.UploadedAt,
                    UserId = userId
                    
                };

                unitOfWork.ImageService.Insert(image);
                unitOfWork.Save();

                ImageResponseDTO responseDTO = unitOfWork.ImageService.GetImageResponseDTO(image);

                return Ok(new Response<ImageResponseDTO>(responseDTO,"Image Successfully Inserted"));  
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
