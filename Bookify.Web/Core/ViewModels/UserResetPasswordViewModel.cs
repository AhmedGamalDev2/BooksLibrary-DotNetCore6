namespace Bookify.Web.Core.ViewModels
{
    public class UserResetPasswordViewModel
    {

        public string Id { get; set; } = null!;
        [DataType(DataType.Password),
                 StringLength(100, ErrorMessage = Errors.MaxMinLenth, MinimumLength = 8),
                 RegularExpression(pattern: RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password),
            Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch), Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
