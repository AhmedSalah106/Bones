using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Implementation
{
    public class AdminRepository: Repository<Admin> ,IAdminRepository
    {
        private readonly BonesContext context;
        public AdminRepository(BonesContext context):base(context)
        {
            this.context = context;
        }

        public Admin GetByUserId(string UserId)
        {
            return context.Admins.FirstOrDefault(a => a.UserId == UserId);
        }
    }
}
