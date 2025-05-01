using Bones_App.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml;

namespace Bones_App.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedAdmin(IUnitOfWork unitOfWork,BonesContext context)
        {

            ApplicationUser user = await unitOfWork.UserManager.FindByEmailAsync("Admin@gmail.com");
            if (user == null)
            {

                await unitOfWork.UserManager.CreateAsync(user, "Admin123!@#");
                await unitOfWork.UserManager.AddToRoleAsync(user, "Admin");
                Admin admin = new Admin()
                {
                    Name = user.UserName,
                    Email = user.Email,
                    Password = "Admin123!@#",
                    User = user,
                    UserId = user.Id
                };
                context.Admins.Add(admin);
                context.SaveChanges();
            }
            else
            {
                Admin admin = unitOfWork.AdminService.GetById(1);
                admin.User = user;
                admin.UserId = user.Id;
                context.SaveChanges();
            }



        }
    }
}
