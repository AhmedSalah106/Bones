using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Hubs;
using Bones_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bones_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<ChatHub> hubContext;
        public ChatController(IUnitOfWork unitOfWork,IHubContext<ChatHub>hubContext)
        {
            this.hubContext = hubContext;
            this.unitOfWork = unitOfWork;
        }


        [HttpGet("GetChat")]
        public IActionResult GetChat([FromQuery]GetChatDTO chatDTO)
        {
            try
            {
                string SenderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<MessageDTO> messages = unitOfWork.ChatService.GetMessages(SenderId,chatDTO.ReceiverId);
                if(messages==null || messages.Count == 0)
                {
                    return NotFound(new Response<string>("No Messages Yet"));
                }

                return Ok(new Response<List<MessageDTO>>(messages,"Messages successfully retrieved"));
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
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(MessageDTO messageDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(new Response<string>(ModelState.ToString()));
                }
                var SenderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Message message = unitOfWork.ChatService.ConvertFromMessageDTOToMessage(messageDTO);
                message.SenderId = SenderId;

                unitOfWork.ChatService.Insert(message);
                unitOfWork.Save();

                await hubContext.Clients.User(SenderId).SendAsync("ReceiveMessage", SenderId, messageDTO.Content);
                await hubContext.Clients.User(messageDTO.ReceiverId).SendAsync("ReceiveMessage",SenderId, messageDTO.Content);

                return Ok(new Response<MessageDTO>(messageDTO,"Message Successfully Sent"));

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

        [HttpGet("GetAllMessages")]
        public IActionResult GetAllMessages()
        {
            List<Message> messages = unitOfWork.ChatService.GetAll();
            return Ok(new Response<List<Message>>(messages));
        }
    }
}
