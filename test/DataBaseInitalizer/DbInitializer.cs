using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using test.Utility;

namespace test.DataBaseInitalizer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManger;
        private readonly UserManager<IdentityUser> userManger;

        public DbInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManger,UserManager<IdentityUser> userManger)
        {
            this.context = context;
            this.roleManger = roleManger;
            this.userManger = userManger;
        }
        //public async void Initialize()
        //{

        //    if (context.Database.GetPendingMigrations().Count() != 0)
        //    {
        //        context.Database.Migrate();
        //    }

        //    if (!await roleManger.RoleExistsAsync(SD.Role_Admin))
        //    {
        //        await roleManger.CreateAsync(new IdentityRole(SD.Role_Admin));
        //        await roleManger.CreateAsync(new IdentityRole(SD.Role_Employee));
        //        await roleManger.CreateAsync(new IdentityRole(SD.Role_User_comp));
        //        await roleManger.CreateAsync(new IdentityRole(SD.Role_User_indi));

        //        var user = new ApplicationUser
        //        {
        //            UserName = "admin@gmail.com",
        //            Email = "admin@gmail.com",
        //            Name = "hema",
        //            PhoneNumber = "+200000",
        //            StreetAddress = "zafar",
        //            City = "Mansoura",
        //            State = "Usa",
        //            PostalCode = "2222"
        //        };

        //        await userManger.CreateAsync(user, "Hema123!");

        //        user = context.ApplicationUsers.FirstOrDefault(u => u.Email == user.Email);

        //        await userManger.AddToRoleAsync(user, SD.Role_Admin);

        //    }
        //}

        public void Initialize()
        {

            if (context.Database.GetPendingMigrations().Count() != 0)
            {
                context.Database.Migrate();
            }

            if (! roleManger.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                 roleManger.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                 roleManger.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                 roleManger.CreateAsync(new IdentityRole(SD.Role_User_comp)).GetAwaiter().GetResult();
                 roleManger.CreateAsync(new IdentityRole(SD.Role_User_indi)).GetAwaiter().GetResult();

                var user = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name = "hema",
                    PhoneNumber = "+200000",
                    StreetAddress = "zafar",
                    City = "Mansoura",
                    State = "Usa",
                    PostalCode = "2222"
                };

                 userManger.CreateAsync(user, "Hema123!").GetAwaiter().GetResult();

                user = context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com").GetAwaiter().GetResult();

                 userManger.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            }
        }
    }
}
