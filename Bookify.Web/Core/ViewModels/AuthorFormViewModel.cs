using Bookify.Web.Core.Consts;

namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = Errors.MaxLenth),  Display(Name = "Author")]
        [Remote("AllowAuthor", "Authors", AdditionalFields = "Id", ErrorMessage = Errors.Dublicated)]
        [RegularExpression(RegexPatterns.CharactersOnly_Eng,ErrorMessage =Errors.OnlyEnglishLetters)]
        public string Name { get; set; } = null!;
    }
}