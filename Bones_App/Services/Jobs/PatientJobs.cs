using Bones_App.Helpers;

namespace Bones_App.Services.Jobs
{
    public class PatientJobs
    {
        private readonly IUnitOfWork unitOfWork;
        public PatientJobs(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void DeletePatient(int Id)
        {
            unitOfWork.PatientService.Delete(Id);
            unitOfWork.Save();
        }
    }
}
