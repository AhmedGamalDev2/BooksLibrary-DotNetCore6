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
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        /*the UserManger is for admins => how admins deal with users.
         but User Account for themselves users => how users change them avater image or change password or change username.....
         */

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
        [AjaxOnly]
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
            // var user = _mapper.Map<ApplicationUser>(model); //do not do that
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

            return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));


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


        [HttpGet]
        //[AjaxOnly]
        public async Task<IActionResult> Edit(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            
            if (user is null)
                return NotFound();
            
            var viewmodel = _mapper.Map<UserFormViewModel>(user);
            var userRoles = await _userManager.GetRolesAsync(user); //Roles that user assigned in 
            viewmodel.SelectedRoles = userRoles; 
           
            var generalRoles =  _roleManager.Roles;
            viewmodel.Roles = generalRoles.Select(r=>new SelectListItem // هنا يملأ القائمة المنسدلة
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            return PartialView("Form", viewmodel);
           
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserFormViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user is null)
            {
                return NotFound();
            }
           
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
             user = _mapper.Map(model, user);//replaced 5 elements=>fullname,username,email,normalizedemail,normalizedusername   //source:model, dest:user 

            user.LastUpdatedOn = DateTime.Now;
            user.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var updated = await _userManager.UpdateAsync(user);
            if (updated.Succeeded)
            {
                var oldUserRoles = await _userManager.GetRolesAsync(user);
                var isRolesUpdated = !oldUserRoles.SequenceEqual(model.SelectedRoles);//check if there difference between two elements or not //source: oldUserRoles,second:selectedRoles
                if (isRolesUpdated) //in case  there are difference
                {
                    var resultRemove = await _userManager.RemoveFromRolesAsync(user, oldUserRoles);
                    var resultAdd = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                }
                var viewModel = _mapper.Map<UserViewModel>(user);
                return PartialView("_UserRow", viewModel);
            }
            //if we used automapper here
 
            return BadRequest();
            #region Note
            //when you deal with Identity with edit action don't forget edit  (NormalizedEmail,NormalizedUserName) in mapping prifile or in edit action 
            //await _userManager.NormalizeEmail(); //if in edit action
            //await _userManager.NormalizeName();//if in edit action
            #endregion
            #region another solution
            /**
             *  var user = await _userManager.FindByIdAsync(model.Id);

            if (user is null)
            {
                return NotFound();
            }
           
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //if we used automapper here
            var oldUserRoles =  await _userManager.GetRolesAsync(user);
            var resultRemove  = await _userManager.RemoveFromRolesAsync(user, oldUserRoles);
            if (resultRemove.Succeeded)
            {
                var resultAdd = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                if (resultAdd.Succeeded)
                {
                    user.FullName = model.FullName;
                    user.UserName = model.Username;
                    user.Email = model.Email;
                    user.LastUpdatedOn = DateTime.Now;
                    user.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                    var viewModel = _mapper.Map<UserViewModel>(user);
                    await _userManager.UpdateAsync(user);
                    return PartialView("_UserRow", viewModel);
                }
            }
            return BadRequest();
             */
            #endregion
        }


        /***************************************************************/

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            user.LastUpdatedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);//inestead of  _context.SaveChanges();
            return Ok(user.LastUpdatedOn.ToString());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            var isLocked = await _userManager.IsLockedOutAsync(user);
            if (isLocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            return Ok();
            #region another way
            /*
              var endDate =  await _userManager.GetLockoutEndDateAsync(user);
            if (endDate.HasValue && endDate > DateTimeOffset.Now)
            {
                // تعيين تاريخ انتهاء القفل إلى الوقت الحالي أو أي تاريخ في المستقبل
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                await _userManager.ResetAccessFailedCountAsync(user); // إعادة تعيين عداد الفشل في تسجيل الدخول
                return Ok("Unlocked");
            }
            return BadRequest("This user already non locked");
             */
            #endregion
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> GetLastUpdatedOn(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();
            var lastUpdated = user.LastUpdatedOn;
            return Json(lastUpdated.ToString());
        }

        [HttpGet]
        [AjaxOnly]

        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound();
            var viewModel = new UserResetPasswordViewModel { Id = user.Id };
            return PartialView("ResetPasswordForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(UserResetPasswordViewModel model)
        {//like Edit
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user is null)
                return NotFound();
            if (model.Password == model.ConfirmPassword)
            {
                var oldPasswordHash = user.PasswordHash;
                await _userManager.RemovePasswordAsync(user);

                var result = await _userManager.AddPasswordAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user.LastUpdatedById = User?.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                    user.LastUpdatedOn = DateTime.Now;

                    var viewModel = _mapper.Map<UserViewModel>(user);

                    await _userManager.UpdateAsync(user);//you must write this //inestead of  _context.SaveChanges();

                    return PartialView("_UserRow", viewModel);

                }
                user.PasswordHash = oldPasswordHash;
                await _userManager.UpdateAsync(user);//you must write this //inestead of  _context.SaveChanges();

                //return BadRequest();
                return BadRequest(string.Join(',', result.Errors.Select(e => e.Description)));

            }
            return BadRequest();
        }
    }


}
