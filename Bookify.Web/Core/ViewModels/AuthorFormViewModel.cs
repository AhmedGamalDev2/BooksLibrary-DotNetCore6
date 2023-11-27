namespace Bookify.Web.Core.ViewModels
{
    public class AuthorFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Max length cannot be more than 100 chr.")]
        [Remote("AllowAuthor", "Authors", AdditionalFields = "Id", ErrorMessage = "Author with the same name is already existed!")]
        public string Name { get; set; } = null!;
    }
}