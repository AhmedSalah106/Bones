using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Implementation
{
    public class ChatRepository:Repository<Message>,IChatRepository
    {
        private readonly BonesContext context;
        public ChatRepository(BonesContext context):base(context) 
        {
            this.context = context;
        }
    }
}
