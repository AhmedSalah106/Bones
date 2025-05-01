using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ImageController(IUnitOfWork unitOfWork
            , IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("RetrieveImages")]
        public IActionResult RetrieveImage(RetrieveImageDTO imageDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new Response<string>( ModelState.ToString()));
                }

                List<ImageResponseDTO> Images = unitOfWork.ImageService.GetAllUserImagesDTO(imageDTO.UserId);

                if(Images==null||Images.Count==0)
                {
                    return NotFound(new Response<string>( "User has not uploaded any images Yet!"));
                }


                return Ok(new Response<List<ImageResponseDTO>>(Images, "Successfully Retrieve all Images"));

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

        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromForm] UploadImageDTO imageDTO) 
        {
            try
            {
        
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
                    PatientID = imageDTO.UserId,
                    UploadedAt = imageDTO.UploadedAt
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
