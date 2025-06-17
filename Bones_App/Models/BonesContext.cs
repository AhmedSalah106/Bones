using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bones_App.Models
{
    public class BonesContext : IdentityDbContext<ApplicationUser>  
    {
        public BonesContext(DbContextOptions options) :base(options) { }
        
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Specialist> Specialists { get; set; }
        public DbSet<Emails> Emails { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SpecialistRate> SpecialistRates { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    }
}
