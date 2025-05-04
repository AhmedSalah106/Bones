using Bones_App.DTOs;
using Bones_App.Handlers.QueriesHandler.PatientQueriesHandler;
using Bones_App.Helpers;
using Bones_App.Queries.SpecialistQueries;
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

        private readonly IMediator mediator;
        public SpecialistController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Query = new GetAllQuery();
                List<SpecialistResponseDTO> specialists =
                    await mediator.Send(Query);

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
