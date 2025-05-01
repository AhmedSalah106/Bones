using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Implementation
{
    public class EmailRepository:Repository<Emails> , IEmailRepository
    {
        private readonly BonesContext context;
        public EmailRepository(BonesContext context):base(context) 
        {
            this.context = context;
        }
    }
}
