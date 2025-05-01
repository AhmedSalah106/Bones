using Bones_App.Models;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Interfaces
{
    public interface IPatientRepository:IRepository<Patient>
    {
        List<Patient> GetAll(string Include);
        Patient GetById(int Id,string Include);
        Patient GetByUserId (string UserId);
    }
}