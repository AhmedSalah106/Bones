using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Queries.PatientQuries;
using Bones_App.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.Tasks;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class PatientController : ControllerBase
    {
        private readonly IMediator mediator;
        public PatientController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var Query = new GetAllQuery();

                List<PatientResponseDTO> patientResponseDTOs =
                   await  mediator.Send(Query);

                if (patientResponseDTOs == null || patientResponseDTOs.Count() == 0)
                {
                    return NotFound(new Response<string>("No Patients Register yet"));
                }

                return Ok(new Response<List<PatientResponseDTO>>(patientResponseDTOs , "Successfully Return All Patients"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while processing your request.",
                    Details = ex.Message
                });
            }
        }
    }
}
