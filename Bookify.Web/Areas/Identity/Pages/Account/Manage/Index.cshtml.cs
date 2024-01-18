// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bookify.Web.Core.Models;
using Bookify.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookify.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IImageService _imageService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required, MaxLength(100, ErrorMessage = Errors.MaxLenth), Display(Name = "Full Name")]
            [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
            public string FullName { get; set; } = null!;
            [Phone]
            [Display(Name = "Phone number"), MaxLength(11, ErrorMessage = Errors.MaxLenth),
                RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidMobileNumber)]
            public string PhoneNumber { get; set; }
            public IFormFile Avatar { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var fullName = user.FullName;
            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FullName = fullName,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            //deal with image (avatar image) using ImageService
            if(Input.Avatar is not null)
            {
                _imageService.Delete($"/images/users/{user.Id}.png");
                var (isUploaded,errorMessage) = await _imageService.UploadAsync(Input.Avatar,$"{user.Id}.png","/images/users",hasThumbnail: false);
                if(!isUploaded)
                {
                    ModelState.AddModelError("Input.Avatar", errorMessage); // you must write like this ModelState.AddModelError("Input.Avatar" => "Input.Avatar" as string  , it not like MVC, (typeof(Input.Avatar) not correct
                    await LoadAsync(user);
                    return Page();
                }
            }
            //else if (Input.ImageRemoved)
            //    _imageService.Delete($"/images/users/{user.Id}.png");


            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            var fullName = user.FullName;
            if (Input.FullName != fullName)
            {
                user.FullName = Input.FullName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set Full Name.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
