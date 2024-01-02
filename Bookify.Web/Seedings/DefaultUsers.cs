namespace Bookify.Web.Seedings
{
    public static class DefaultUsers
    {
        //we must call this function (SeedUser) in program to store user in database //f20
        public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "Admin@bookify.com",
                EmailConfirmed = true,
                FullName = "Admin",
            };

            //if (!userManager.Users.Any()) { await userManager.CreateAsync(admin); }
            //OR
            var user = await userManager.FindByNameAsync(admin.UserName);

            if (user is null)
            {
                    await userManager.CreateAsync(admin, "Admin@@123");
                    await userManager.AddToRoleAsync(admin, AppRoles.Admin);
 
            }

            
        }
    }
}
