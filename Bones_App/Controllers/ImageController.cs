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
        public ImageController(IMediator mediator, IUnitOfWork unitOfWork
            ,IWebHostEnvironment webHostEnvironment)
        {
            this.mediator = mediator;
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }



        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageDTO imageDTO)
        {
            try
            {

                 var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //var userId = "710f901a-ea6d-47e6-ac66-4ef8cb37c428";
                ModelAPIsBatchResponseDTO response = await unitOfWork.ModelIntegrationService.GetModelReport(imageDTO);


                if (response.Is_Success == false)
                    return Ok(new Response<string>(response.Error_Message));


                foreach (var img in imageDTO.ImageFiles)
                {
                    string wRootPathe = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    string ImageName = Guid.NewGuid().ToString() + "_" + img.FileName;
                    string ImagePath = Path.Combine(wRootPathe, ImageName);

                    using (FileStream fileStream = new FileStream(ImagePath, FileMode.Create))
                    {
                        img.CopyTo(fileStream);
                    }

                    Image image = new Image()
                    {
                        ImageURL = Path.Combine("/Images", ImageName),
                        PatientID = imageDTO.Id,
                        UploadedAt = imageDTO.UploadedAt,
                        UserId = userId
                    };

                    unitOfWork.ImageService.Insert(image);
                    unitOfWork.Save();
                }

                List<Guid> ids = response.Data.Select(data => data.Id).ToList();

                List<GetModelReportResponseDTO> reports = new List<GetModelReportResponseDTO>();
                foreach(var id in ids)
                {

                    reports.Add(await unitOfWork.ModelIntegrationService.GetModelResponse(id));
                    await Task.Delay(2000);
                }

                    

                return Ok(new Response<List<GetModelReportResponseDTO>>(reports));
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


        [HttpGet("GetReportById")]
        public async Task<IActionResult> GetReportById(Guid Id)
        {
            try
            {
                var result =await unitOfWork.ModelIntegrationService.GetModelResponse(Id);

                return Ok(new Response<GetModelReportDataDTO>(result.Data));
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
