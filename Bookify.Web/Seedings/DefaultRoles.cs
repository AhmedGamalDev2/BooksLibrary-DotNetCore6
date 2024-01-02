namespace Bookify.Web.Seedings
{
    public  static  class DefaultRoles
    {
        //private static  readonly RoleManager<IdentityRole> roleManager;
        //public    DefaultRoles(RoleManager<IdentityRole> roleManager)
        //{
        //    this.roleManager = roleManager;
        //}
        //DefaultRoles s;

        //we must call this function (SeedRole) in program to store Roles in database
        public  static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
             
           if (!roleManager.Roles.Any())
            {
               await roleManager.CreateAsync(new IdentityRole(roleName: AppRoles.Admin));
               await roleManager.CreateAsync(new IdentityRole(roleName: AppRoles.Archive));
               await roleManager.CreateAsync(new IdentityRole(roleName: AppRoles.Reception));
            }
        }

    }
}
