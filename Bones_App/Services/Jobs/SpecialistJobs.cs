using Bones_App.Helpers;

namespace Bones_App.Services.Jobs
{
    public class SpecialistJobs
    {
        private readonly IUnitOfWork unitOfWork;
        public SpecialistJobs(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void DeleteSpecialist(int Id)
        {
            unitOfWork.SpecialistService.Delete(Id);
            unitOfWork.Save();
        }
    }
}
