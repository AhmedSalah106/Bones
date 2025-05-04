using Bones_App.Models;
using Bones_App.Services.Implementation;
using Bones_App.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Bones_App.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BonesContext context;
        public IImageService ImageService { get; }
        public IPatientService PatientService { get; }
        public ISpecialistService SpecialistService { get; }
        public IChatService ChatService {  get; }
        public IModelIntegrationService ModelIntegrationService { get; }
        public IpaymentTransactionService PaymentTransactionService { get; }
        public SignInManager<ApplicationUser> signInManager { get; }
        public IAdminService AdminService { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public ISpecialistRateService specialistRateService { get; }
        public IEmailService EmailService { get; }
        public IConfiguration configuration { get; }
        public IWebHostEnvironment webHostEnvironment { get; }
        public UnitOfWork(IModelIntegrationService modelIntegrationService
            ,IChatService chatService
            ,IpaymentTransactionService paymentTransactionService
            ,IAdminService adminService
            ,ISpecialistRateService specialistRateService
            ,EmailService emailService
            ,IConfiguration configuration 
            ,IWebHostEnvironment webHostEnvironment
            ,BonesContext context
                         , IImageService imageService
                         , IPatientService patientService
                         , ISpecialistService specialistService
                         , SignInManager<ApplicationUser> signInManager
                         , UserManager<ApplicationUser> userManager
                         , RoleManager<IdentityRole> roleManager)
        {
            this.ModelIntegrationService = modelIntegrationService;
            this.ChatService = chatService;
            this.PaymentTransactionService = paymentTransactionService;
            this.AdminService = adminService;
            this.EmailService = emailService;
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            this.context = context;
            this.ImageService = imageService;
            this.PatientService = patientService;
            this.SpecialistService = specialistService;
            this.signInManager = signInManager;
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            this.specialistRateService = specialistRateService;
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
