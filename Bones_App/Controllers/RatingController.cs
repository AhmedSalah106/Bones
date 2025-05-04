using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public RatingController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("RateSpecialist")]
        public IActionResult RateSpecialist(SpecialistRateDTO rateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {       
                    return BadRequest(new Response<string>(ModelState.ToString()));
                }

                SpecialistRate specialistRate = unitOfWork.specialistRateService.GetPatientRatingForSpecialist(rateDTO.SpecialistId, rateDTO.PatientId);

                if (specialistRate == null)
                {
                    specialistRate = unitOfWork.specialistRateService.ConvertToSpecialistRate(rateDTO);
                    unitOfWork.specialistRateService.Insert(specialistRate);
                }
                else
                {
                    specialistRate.Comment = rateDTO.Comment;
                    specialistRate.RatingValue = rateDTO.RatingValue;
                    specialistRate.RatedAt = rateDTO.RatedAt;
                }

                unitOfWork.Save();

                return Ok(new Response<SpecialistRateDTO>(rateDTO,"Rate Added Successfully"));

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

        [HttpPost("GetSpecialistRate")]
        public IActionResult GetSpecialistRate(int specialistId)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(new Response<string>(ModelState.ToString()));
                }

                int avg = unitOfWork.specialistRateService.GetAverageRating(specialistId);
                

                return Ok(new Response<int>(avg,"Rate Average Returned Successfully"));
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

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try 
            {
                List<SpecialistRate> specialistRates = unitOfWork.specialistRateService.GetAll();
                if (specialistRates == null || specialistRates.Count == 0)
                {
                    return NotFound(new Response<string>("No Rates Added Yet"));
                }

                return Ok(new Response<List<SpecialistRate>>(specialistRates,"All Rates Successfully Retrieved"));

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
        [HttpGet("GetTopTenRatedSpecialists")]
        public IActionResult GetTopTenRatedSpecialists()
        {
            try
            {
                List<SpecialistRateResponseDTO> TopTenSpecialists = unitOfWork.specialistRateService.GetTopTenSpecialists();
                if(TopTenSpecialists==null|| TopTenSpecialists.Count == 0)
                {
                    return NotFound(new Response<string>("No Rates Added Yet"));
                }
                return Ok(new Response<List<SpecialistRateResponseDTO>>(TopTenSpecialists,"Top Ten Successully Retrieved"));
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
