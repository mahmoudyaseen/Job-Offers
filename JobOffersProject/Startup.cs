using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication1.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateInitailRoleUser();
        }

        public void CreateInitailRoleUser()
        {
            //as we ee in identity
            //create rolemanager to manage roles by rolestore that store in db
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if(!roleManager.RoleExists("Admins"))
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admins";
                roleManager.Create(role);
            }
            ApplicationUser user = new ApplicationUser();
            user.Email = "MahmoudYaseen123@Gmail.com";
            user.UserName = "MahmoudYaseen";
            var result = userManager.Create(user , "12345M@dy12345");
            if(result.Succeeded)
            {
                userManager.AddToRole(user.Id, "Admins");
            }
        }
    }
}
