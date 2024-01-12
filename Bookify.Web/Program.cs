using Bookify.Web.Core.Mapping;
using Bookify.Web.Helpers;
using Bookify.Web.Seedings;
using Bookify.Web.Seeds;
using Bookify.Web.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//map data from CloudinarySettings key in appsettings to CloudinarySettings class (f15)
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));//GetSection("CloudinarySettings") from appsettings
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


#region Identity Details
//f20
//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true) //this for only ApplicationUser not for ApplicationUser and IdentityRole
//    .AddEntityFrameworkStores<ApplicationDbContext>();//(1) //AddDefaultIdentity => This function configures the Identity services with a default set of options suitable for most common scenarios. It sets up features like user registration, password management, account confirmation, and other default behaviors.

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultUI()//(2) //this AddDefaultUI => we must write it in case we did not use builder.Services.AddDefaultIdentity<ApplicationUser> (1) , means we use default UI(front) Identity Pages or use Razor Pages and controllers for the Identity UI. like (Login,ResetPassword,Registger...)
   .AddDefaultTokenProviders(); //this AddDefaultTokenProviders => to use forgit password page ,if we must use it if we will forgit password page
                                //This function adds the default token providers used by the Identity system. Token providers are responsible for generating and validating tokens used for various purposes, such as email confirmation and password reset

#region More Details
/*
    ////AddIdentity with this all code and this all configration equals AddDefaultIdentity only ( تقريبا يعني) 
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    { 
    // Password policies
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout policies
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    // Email confirmation
    options.SignIn.RequireConfirmedEmail = true;

    // 2FA
    options.SignIn.RequireConfirmedPhoneNumber = true;

    // Security stamp
    options.SecurityStampValidationInterval = TimeSpan.FromDays(1);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
*/
#endregion
builder.Services.Configure<IdentityOptions>(options =>
{
    //// Password settings.
    //options.Password.RequireDigit = true;
    //options.Password.RequireLowercase = true;
    // options.Password.RequireNonAlphanumeric = true;
    // options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    //    options.Password.RequiredUniqueChars = 1;

    //    // Lockout settings.
    //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //    options.Lockout.MaxFailedAccessAttempts = 5;
    //    options.Lockout.AllowedForNewUsers = true;

    //    // User settings.
         options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
         options.User.RequireUniqueEmail = true;
});
#endregion


builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(assemblies: AssemblyBuilder.GetAssembly(typeof(Mappingprofile)));
builder.Services.AddExpressiveAnnotations();
 
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
 
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#region call SeedRolesAsync and SeedUserAsync functions to save Roles and users in database //f20

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
//To use this (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();) => we must use this (builder.Services.AddIdentity<ApplicationUser,IdentityRole>())
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

await DefaultRoles.SeedRolesAsync(roleManager);
await DefaultUsers.SeedUserAsync(userManager);

 
#endregion
 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
