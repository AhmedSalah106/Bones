using Bones_App.DTOs;
using Bones_App.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ModelController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("PredictSingleImage")]

        public async Task<IActionResult>Predict([FromForm] PredictRequestDTO predictRequest)
        {
            if (predictRequest.imageFile == null || predictRequest.imageFile.Length == 0)
                return BadRequest(new Response<string>("No image provided"));

            var result = await unitOfWork.ModelIntegrationService.SendImageToModelAsync(predictRequest.imageFile);

            if (!result.Is_Success)
                return StatusCode(500, new Response<string>(result.Error_Message ?? "Model API failed"));

            return Ok(new Response<ModelApiData>(result.Data, "Prediction submitted successfully"));
        }

        [HttpPost("PredictSingleImageWithBodyPart")]
        public async Task <IActionResult> PredictSingleImageWithBodyPart([FromForm]PredictImageWithBodyPartDTO predictImageWithBodyPart)
        {
            if (predictImageWithBodyPart.ImageFile == null || predictImageWithBodyPart.ImageFile.Length == 0)
                return BadRequest(new Response<string>("No image provided"));

            var result = await unitOfWork.ModelIntegrationService.SendImageWithBodyPartToModelAsync(predictImageWithBodyPart.BodyPart,predictImageWithBodyPart.ImageFile);

            if (!result.Is_Success)
                return StatusCode(500, new Response<string>(result.Error_Message ?? "Model API failed"));

            return Ok(new Response<ModelApiData>(result.Data, "Prediction submitted successfully"));
        }

        [HttpGet("GetReportById")]
        public async Task<IActionResult> GetReport(Guid ReportId)
        {
            var result = await unitOfWork.ModelIntegrationService.GetModelResponse(ReportId);
            

            return Ok(new Response<GetModelReportDataDTO>(result.Data));

        }
    }
}
