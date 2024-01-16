using Microsoft.Extensions.Options;

namespace Bookify.Web.Helpers
{
    //this class to add new claim to the user 
    //UserClaimsPrincipalFactory => this provides methods to create  a claims principal for a given user
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }
        //this function will call when the program run
        //we added scope for this class in program like this (builder.Services.AddScoped <IUserClaimsPrincipalFactory<ApplicationUser>,ApplicationUserClaimsPrincipalFactory>();)
        //claims is added to user when he login
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user); //get all claims that assigned in this user
            //add new claim (FullName) to user
            identity.AddClaim(new Claim(ClaimTypes.GivenName,user.FullName));//this ClaimTypes.GivenName crears fullname for user
            return identity;
            //here, we can add any another claims 
        }

    }


}
