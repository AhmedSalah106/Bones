using Bones_App.Models;
using Bones_App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bones_App.Helpers
{
    public interface IUnitOfWork
    {
        public void Save();
        public IImageService ImageService { get;}
        public IPatientService PatientService { get;}
        public IpaymentTransactionService PaymentTransactionService { get;}
        public ISpecialistService SpecialistService { get;}
        public IChatService ChatService { get;}
        public SignInManager<ApplicationUser> signInManager { get;}
        public UserManager<ApplicationUser> UserManager { get;} 
        public IEmailService EmailService { get;}
        public ISpecialistRateService specialistRateService { get;}
        public IAdminService AdminService { get;}
        public RoleManager<IdentityRole> RoleManager { get;}
        public IConfiguration configuration { get;}
        public IWebHostEnvironment webHostEnvironment {  get;}
    }
}
