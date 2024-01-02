
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Bookify.Web.Data
{
    #region this take ApplicationUser, ApplicationRole, ApplicationUserClaim, IdentityUserRole, ApplicationUserLogin, ApplicationRoleClaim and ApplicationUserToken
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim, IdentityUserRole<string>, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>

    #endregion
    #region this take IdentityUser and IdentityRole only
    // this take IdentityUser and IdentityRole only
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    //{ 
    #endregion
     public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region DbSet
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCategory> BookCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("SerialNumber", schema: "shared")
                .StartsAt(1000001);
            builder.Entity<BookCopy>()
                           .Property(e => e.SerialNumber)
                           .HasDefaultValueSql("NEXT VALUE FOR shared.SerialNumber");

            builder.Entity<BookCategory>(e => e.HasKey(b => new { b.BookId, b.CategoryId })); // assign primary key for BookCategory entity 
            base.OnModelCreating(builder);

            #region (7 seven Tablses Identity or Domain Models Identity ) Rename tables
            /*
            // There is problem(don't work) when Rename tables and extend Identity classes in the same time 
            //=> no problem when rename tables but we must user new name for IdentityUser (ApplicationUser ) not old name (IdentityUser) and so on IdnetityRole
            // لازما نستخدم الاسماء الجديد للكلاسات اللي عايزين نغير اسمها في الداتابيز => ApplicationUser instead of IdentityUser
            //Replace AspNetUsers table Name in database to name( Users) and under Security sechema instead of dbo schema
            //IdentityUser is class or Domain Model that Meet face to face with AspNetUsers table in database
            //IdentityUser is Domain Model for table in database called AspNetUsers
            //like:: Domain Model(Book) for table (Books) in database sqlserver
            builder.Entity<ApplicationUser>().ToTable("Users", "Security");// instead of builder.Entity<IdentityUser>().ToTable("Users", "Security");

            //Replace AspNetRoles table Name in database to name( Roles) and under Security sechema instead of dbo schema
            //IdentityRoles is class or Domain Model that Meet face to face with AspNetRoles table in database
            builder.Entity<IdentityRole>().ToTable("Roles", "Security");

            //Replace AspNetUserRoles table Name in database to name( UserRoles) and under Security sechema instead of dbo schema
            //IdentityUserRole is class or Domain Model that Meet face to face with AspNetUserRoles table in database
            //string :: point to that IdentityUserRole table or class use a key of type string like Id ,
            //<string> indicates the type of the user's key, and in the default configuration of ASP.NET Core Identity, the key is the Id property of the user.
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Security");

            //Replace AspNetUserClaims table Name in database to name( UserClaims) and under Security sechema instead of dbo schema
            //IdentityUserClaim is class or Domain Model that Meet face to face with AspNetUserClaims table in database
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Security");

            //Replace AspNetRoleClaims table Name in database to name( RoleClaims) and under Security sechema instead of dbo schema
            //IdentityRoleClaim is class or Domain Model that Meet face to face with AspNetRoleClaims table in database
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Security");

            //Replace AspNetUserLogins table Name in database to name( UserLogins) and under Security sechema instead of dbo schema
            //IdentityUserLogin is class or Domain Model that Meet face to face with AspNetUserLogins table in database
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Security");

            //Replace AspNetUserTokens table Name in database to name( UserTokens) and under Security sechema instead of dbo schema
            //IdentityUserToken is class or Domain Model that Meet face to face with AspNetUserTokens table in database
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Security");
            //then create migration and updatedatabase
         */
            #endregion
            #region Remove column from AspNetUsers table , but don't do that,don't do that , must not remove any column from Identity tables
            //builder.Entity<IdentityUser>() ,DevCreed says don't do that
            //    .Ignore(e => e.PhoneNumber)
            //    .Ignore(e => e.PhoneNumberConfirmed);
            #endregion

            #region Details about Identity
            /*
             * انا معنديش الا الثلاثة كلاسات دول علشان يتعاملوا مع 
             *identity وهم ::
             *UserManager and RoleManager and SignInManager
             **********************
             * UserManager<ApplicationUser>:
             * 
                Role: Manages user-related operations such as creating, updating, deleting, and finding users.
                Responsibilities:
                Create new users.
                Manage user properties (e.g., email, password).
                Find users based on various criteria.
                Delete users.

              *RoleManager<ApplicationRole>:
              *
                Role: Manages role-related operations such as creating, updating, deleting, and finding roles.
                Responsibilities:
                Create new roles.
                Manage role properties.
                Find roles based on various criteria.
                Delete roles.

             *SignInManager<ApplicationUser>:

                Role: Manages sign-in related operations, including authentication and external login.
                Responsibilities:
                Authenticate users.
                Sign in users.
                Sign out users.
                Manage external logins (e.g., Google, Facebook).
              */
            #endregion
        }

    }
}