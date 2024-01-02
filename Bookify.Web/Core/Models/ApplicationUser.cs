
namespace Bookify.Web.Core.Models
{
    //Extend Users Entity or IdentityUser Domain Model or AspNetUsers table
    public class ApplicationUser : IdentityUser
    {
        /*you should add this class (ApplicationUser) in any case => before adding Scaffolding for Identity ,
         * ,until if you don't add more properties like FullName or IsDeleted 
         * , because after that ,you may need add some properties 
         */
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }

        #region Who did created and lastupated  this user 
        /* do not write navigation property because it occur problem 
         * ,only write string? CreatedById and string? LastUpdatedById
         * Self Relationship
         */
        public string? CreatedById { get; set; }

        public string? LastUpdatedById { get; set; }
        #endregion



    }



    #region To extend remained Identity classes
    /*
     
     *There is problem(don't work) when Rename tables and extend Identity classes in the same time 
    *Extend Roles Entity or table
    *To apply these extends ,you must write this: (builder.Services.AddIdentity<ApplicationUser, ApplicationRole>() .AddEntityFrameworkStores<ApplicationDbContext>();)  in program 
     
    public class ApplicationRole : IdentityRole
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
    //Extend RoleClaims Entity or AspNetRoleClaims table
    public class ApplicationUs : IdentityRoleClaim<string>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
    //Extend UserClaims Entity or AspNetUserClaims table
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
    //Extend RoleClaims Entity or AspNetRoleClaims table
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public bool IsDeletedRoleClaim { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
    //Extend UserLogins Entity or AspNetUserLogins table
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
    //Extend UserTokens Entity or AspNetUserTokens table
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
     */
    #endregion



}
