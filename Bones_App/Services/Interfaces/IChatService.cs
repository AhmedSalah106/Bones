using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Interfaces
{
    public interface IChatService:IService<Message>
    {
        Message ConvertFromMessageDTOToMessage(MessageDTO messageDTO);
        List<GetChatResponseDTO> GetMessages(string SenderID, string ReceiverId);
        MessageDTO ConvertFromMessageToMessageDTO(Message message);
    }
}
