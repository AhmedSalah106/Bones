using Bones_App.DTOs;
using Bones_App.Helpers;
using Bones_App.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
namespace Bones_App.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IUnitOfWork unitOfWork;
        public ChatHub(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task SendMessage(MessageDTO messageDTO)
        {

           
            Message message = unitOfWork.ChatService.ConvertFromMessageDTOToMessage(messageDTO);

            var SenderId = Context.UserIdentifier;

            message.SenderId = SenderId;

            unitOfWork.ChatService.Insert(message);
            unitOfWork.Save();

            await Clients.User(SenderId).SendAsync("ReceiveMessage", SenderId, messageDTO.Content);
            await Clients.User(messageDTO.ReceiverId).SendAsync("ReceiveMessage", SenderId, messageDTO.Content);

        }
    }
}
