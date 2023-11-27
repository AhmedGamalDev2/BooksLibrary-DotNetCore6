namespace Bookify.Web.Core.ViewModels
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Max length cannot be more than 100 chr.")]
        [Remote("AllowCategory", "Categories" ,AdditionalFields ="Id",ErrorMessage ="Category with the same name is already existed!")]
        public string Name { get; set; } = null!;
    }
}