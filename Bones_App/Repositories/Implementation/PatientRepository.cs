using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Bones_App.Repositories.Implementation
{
    public class PatientRepository : Repository<Patient> , IPatientRepository 
    {
        private readonly BonesContext context;

        public PatientRepository(BonesContext context):base(context)
        {
            this.context = context;
        }

        public List<Patient> GetAll(string Include)
        {
            return context.Patients.Include(Include).ToList();
        }

        public Patient GetById(int Id, string Includes)
        {
            return context.Patients.Include(Includes).FirstOrDefault(e=> e.Id==Id);
        }

        public Patient GetByUserId(string UserId)
        {
            return context.Patients.Include(e=>e.User).FirstOrDefault(u => u.UserId == UserId);
        }
    }
}
