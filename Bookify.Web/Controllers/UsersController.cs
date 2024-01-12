using Bookify.Web.Core.Consts;
using Bookify.Web.Core.Models;
using Bookify.Web.Core.ViewModels;
using Bookify.Web.Filters;
using Bookify.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles =AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _context = context;
            _emailSender = emailSender;

        }

        public async Task<IActionResult> Index()
        {


            var users = await _userManager.Users.ToListAsync();
            var usersViewModel = _mapper.Map<IEnumerable<UserViewModel>>(users);
            return View(usersViewModel);
        }

        [HttpGet]
        //[AjaxOnly]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync(); //we can't send domain model but send view model
            var userFormViewModel = new UserFormViewModel
            {
                #region first way to map IdentityRole to SelectListItem with automaper(1)
                ////var roles = await _roleManager.Roles.ToListAsync();
                //Roles = _mapper.Map<IEnumerable<SelectListItem>>(roles)
                #endregion
                #region second way to map IdentityRole to SelectListItem(2)
                /*
                ////var roles = await _roleManager.Roles.ToListAsync();
                Roles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })*/
                #endregion
                #region third way to map IdentityRole to SelectListItem(3)
                Roles = await _roleManager.Roles //without this line : var roles =  await _roleManager.Roles.ToListAsync(); 
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToListAsync()
                #endregion

            };
            return PartialView("Form", userFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var checkuser = await _userManager.FindByEmailAsync(model.Email);
            if (checkuser != null)
            {
                return BadRequest("this email existed");
            }
            #region Note (don't use automapper with ApplicationUser as Destination => only use new ApplicationUser
            // var user = _mapper.Map<ApplicationUser>(user); //do not do that
            //here we can not use automapper with ApplicationUser as Destination with UserFormViewModel
            //because automapper will initialize Id (in ApplicationUser) with null
            //but when using new keyword (in new ApplicationUser) ,it(new) will initialize Id (in ApplicationUser) with (new Guid)
            #endregion
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Username,
                Email = model.Email,
                CreatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                var viewmodel = _mapper.Map<UserViewModel>(user);
                user.CreatedOn = DateTime.Now;
                user.IsDeleted = false;

                return PartialView("_UserRow", viewmodel);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                      .ToList();

            return BadRequest(string.Join(',',result.Errors.Select(e=>e.Description)));


            #region Ways to get current(loged) User => UserId , UserName,UserEmail...
            /*
            var UserId = User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")!.Value;
            var UserName = User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")!.Value;
            var UserEmail = User.Claims.FirstOrDefault(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")!.Value;
            var UserSecurityStamp = User.Claims.FirstOrDefault(p => p.Type == "AspNet.Identity.SecurityStamp")!.Value;
            var UserRole = User.Claims.FirstOrDefault(p => p.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")!.Value;
            var nameidentifierType = User.Claims.FirstOrDefault()?.Type;// =http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
            var nameidentifierValue = User.Claims.FirstOrDefault()?.Value;//give NameIdentifier value

            var UserId2 = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var UserName2 = User.FindFirst(ClaimTypes.Name)!.Value;
            var UserEmail2 = User.FindFirst(ClaimTypes.Email)!.Value;
            var UserRole2 = User.FindFirst(ClaimTypes.Role)!.Value;

            var Current_UserName = User?.Identity?.Name;
            var UserId3 = (await _userManager.FindByNameAsync(User?.Identity?.Name)).Id;
            var UserFullName3 = (await _userManager.FindByNameAsync(User?.Identity?.Name)).FullName;
            var UserEmail3= (await _userManager.FindByNameAsync(User?.Identity?.Name)).Email;
             */
            #endregion
        }//end

        
        public async Task<IActionResult> IsEmailExisted(UserFormViewModel model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            var isExisted = user is null || user.Id.Equals(model.Id);
            return Json(isExisted);
        }
          public async Task<IActionResult> IsUserNameExisted(UserFormViewModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Username);

            var isExisted = user is null || user.Id.Equals(model.Id);
            return Json(isExisted);
        }


    }


}
