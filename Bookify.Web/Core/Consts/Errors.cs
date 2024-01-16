namespace Bookify.Web.Core.Consts
{
    public static class Errors
    {
        public const string MaxLenth = "Length can't be more than {1} characters";
        public const string RequiredField = "The {0} field is required";
        public const string Dublicated = "This {0} with the same name is already existed!";
        public const string DublicatedBook = "Book  with the same title is already existed with the same author!";
        public const string NotAllowedExtenstions = "Only .png ,.jpg, .jpeg files are allowed!";
        public const string MaxSize = "File cannot be more than 2MB!";
        public const string NotAllowFutureDates = "Date cannot be in the future!";
        public const string InvalidRange = "{0} should be between {1} and {2}!";
        public const string MaxMinLenth = "The {0} must be at least {2} and at max {1} characters long.";
        public const string ConfirmPasswordNotMatch = "The password and confirmation password do not match.";
        public const string WeakPassword = "passwords contain an uppercase character, lowercase character, a digit, and a non-alphanumeric character. Passwords must be at least 8 characters long.";
        public const string DublicatedEmail = "This email is already existed";
        public const string InvalidUserName = "Username can only contain english letters and digits";
        public const string OnlyEnglishLetters = "Only English letters are allowed.";
        public const string OnlyArabicLetters = "Only Arabic letters are allowed.";
        public const string OnlyNumbersAndLetters = "Only Arabic/English letters or digits are allowed.";
        public const string DenySpecialCharacters = "Special characters are not allowed.";


    }
}
