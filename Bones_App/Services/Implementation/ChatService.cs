using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class ChatService:Service<Message>,IChatService
    {
        private readonly IChatRepository chatRepository;
        public ChatService(IChatRepository chatRepository):base(chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public Message ConvertFromMessageDTOToMessage(MessageDTO messageDTO)
        {
            Message message = new Message()
            {
                Content = messageDTO.Content,
                ReceiverId = messageDTO.ReceiverId,
                SentAt = messageDTO.SentAt
            };

            return message;
        }

        public MessageDTO ConvertFromMessageToMessageDTO(Message message)
        {
            MessageDTO messageDTO = new MessageDTO()
            {
                Content = message.Content,
                ReceiverId = message.ReceiverId,
                SentAt = message.SentAt
            };

            return messageDTO;
        }

        public List<MessageDTO> GetMessages(string SenderID, string ReceiverId)
        {
            List<MessageDTO>messages = chatRepository.GetAll()
                .Where(msg=>(msg.SenderId == SenderID && msg.ReceiverId == ReceiverId)
                    ||(msg.SenderId==ReceiverId&&msg.ReceiverId==SenderID))
                        .OrderBy(msg=>msg.SentAt)
                         .Select(img=>ConvertFromMessageToMessageDTO(img)).ToList();

            return messages;
        }
    }
}
