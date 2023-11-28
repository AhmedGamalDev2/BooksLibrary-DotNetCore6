using Bookify.Web.Core.Consts;

namespace Bookify.Web.Core.ViewModels
{
    public class CategoryFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = Errors.MaxLenth ),Display(Name="Category") ]
        [Remote("AllowCategory", "Categories" ,AdditionalFields ="Id",ErrorMessage =Errors.Dublicated)]
        public string Name { get; set; } = null!;
    }
}