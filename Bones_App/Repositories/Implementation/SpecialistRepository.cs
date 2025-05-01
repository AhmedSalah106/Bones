using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;
using Microsoft.EntityFrameworkCore;

namespace Bones_App.Repositories.Implementation
{
    public class SpecialistRepository: Repository<Specialist> , ISpecialistReposiotry
    {
        private readonly BonesContext context;
        public SpecialistRepository(BonesContext context):base(context)
        {
            this.context = context;
        }

        public Specialist GetByUserId(string UserId)
        {
            return context.Specialists.Include(i=>i.User).FirstOrDefault(u => u.UserId == UserId);
        }
    }
}
