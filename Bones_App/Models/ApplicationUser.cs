using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bones_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName {  get; set; } 
        public bool IsVerified {  get; set; }
    }
}
