using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Bones_App.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public PatientController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<PatientResponseDTO> patientResponseDTOs = 
                    unitOfWork.PatientService.GatAllPatientResponseDTOs();

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
