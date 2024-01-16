using Bookify.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels
{
    public class UserFormViewModel
    {
         
        public string? Id { get; set; }
        [MaxLength(100,ErrorMessage =Errors.MaxLenth),Display(Name ="Full Name")]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng,ErrorMessage =Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;
        [MaxLength(20,ErrorMessage =Errors.MaxLenth)]
        [Remote("IsUserNameExisted", "Users", AdditionalFields = "Id", ErrorMessage = Errors.Dublicated)]
        [RegularExpression(RegexPatterns.Username,ErrorMessage =Errors.InvalidUserName)]
        public string Username { get; set; } = null!;
        [MaxLength(200,ErrorMessage =Errors.MaxLenth), EmailAddress]
         
        [Remote("IsEmailExisted", null!, AdditionalFields = "Id", ErrorMessage = Errors.Dublicated)]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password),
            StringLength(100, ErrorMessage = Errors.MaxMinLenth, MinimumLength = 8),
             RegularExpression(pattern: RegexPatterns.Password,ErrorMessage =Errors.WeakPassword)]
        [RequiredIf("Id == null", ErrorMessage = Errors.RequiredField)] //this (RequiredIf("Id == null") in case of create not edit
        public string? Password { get; set; } 
        
        [DataType(DataType.Password),
            Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch),Display(Name = "Confirm password")]
        [RequiredIf("Id == null", ErrorMessage =Errors.RequiredField)]//this (RequiredIf("Id == null") in case of create not edit
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>(); //SelectedCategoryIds
        public IEnumerable<SelectListItem>? Roles { get; set; } //// another way , may be  public IEnumerable<Category>? Categories { get; set; }

        /********************************************/
        //public UserFormViewModel()
        //{
        //    if (Id != null)
        //    {
        //        Password = null!;
        //        ConfirmPassword = null!;
        //    }
        //}
    }
}
