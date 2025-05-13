using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bones_App.Services.Implementation
{
    public class ChatService:Service<Message>,IChatService
    {
        private readonly IChatRepository chatRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public ChatService(IChatRepository chatRepository , UserManager<ApplicationUser> userManager):base(chatRepository)
        {
            this.userManager = userManager;
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

        public async Task<List<UserNameWithIdDTO>>GetAllPatientsNamesWhoSpokeToSpecialist(string SpecialistId)
        {
            List<string> PatientsIDs = GetAll()
                                           .Where(p=>(p.ReceiverId == SpecialistId)||(p.SenderId == SpecialistId))
                                             .Select(p=>p.SenderId==SpecialistId?p.ReceiverId:p.SenderId).Distinct().ToList();

            List<UserNameWithIdDTO> PatientsNames = new List<UserNameWithIdDTO>();
            foreach (var PatientId in PatientsIDs)
            {
                UserNameWithIdDTO data = new UserNameWithIdDTO();
                data.Id = PatientId;
                ApplicationUser user = await userManager.FindByIdAsync(PatientId);
                data.Name = user.FullName;
                PatientsNames.Add(data);
            }

            return PatientsNames;
        }
        
        public List<GetChatResponseDTO> GetMessages(string SenderID, string ReceiverId)
        {
            List<GetChatResponseDTO>messages = chatRepository.GetAll()
                .Where(msg=>(msg.SenderId == SenderID && msg.ReceiverId == ReceiverId)
                    ||(msg.SenderId==ReceiverId&&msg.ReceiverId==SenderID))
                        .OrderByDescending(msg=>msg.SentAt)
                         .Select(img=> new GetChatResponseDTO { Content = img.Content , ReceiverId = img.ReceiverId, SenderId = img.SenderId , SentAt = img.SentAt} ).ToList();

            return messages;
        }
    }
}
