using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels
{
    public class UserEditFormViewModel
    {
        [RequiredIf("")]
        public string? Id { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLenth), Display(Name = "Full Name")]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
        public string FullName { get; set; } = null!;
        [MaxLength(20, ErrorMessage = Errors.MaxLenth)]
        [Remote("IsUserNameExisted", "Users", AdditionalFields = "Id", ErrorMessage = Errors.Dublicated)]
        [RegularExpression(RegexPatterns.Username, ErrorMessage = Errors.InvalidUserName)]
        public string Username { get; set; } = null!;
        [MaxLength(200, ErrorMessage = Errors.MaxLenth), EmailAddress]

        [Remote("IsEmailExisted", null!, AdditionalFields = "Id", ErrorMessage = Errors.Dublicated)]
        public string Email { get; set; } = null!;

        [Display(Name = "Roles")]
        public IList<string> SelectedRoles { get; set; } = new List<string>(); //SelectedCategoryIds
        public IEnumerable<SelectListItem>? Roles { get; set; } //// another way , may be  public IEnumerable<Category>? Categories { get; set; }


    }
}
