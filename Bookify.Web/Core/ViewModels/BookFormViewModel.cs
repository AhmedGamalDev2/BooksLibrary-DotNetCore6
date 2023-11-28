using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; } //for edit
        [MaxLength(500, ErrorMessage = Errors.MaxLenth)]
        public string Title { get; set; }
        [Display(Name ="Author")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; } // another way , may be  public IEnumerable<Author>? Authors { get; set; }
        [MaxLength(200, ErrorMessage = Errors.MaxLenth)]
        public string Publisher { get; set; } = null!;
        [Display(Name = "Publishing Date")]
        public DateTime PublishingDate { get; set; }

        public IFormFile? Image { get; set; }
        [MaxLength(50, ErrorMessage = Errors.MaxLenth)]
        public string Hall { get; set; } = null!;
        [Display(Name = "Is available for rental?")]
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public IList<int> SelectedCategoryIds { get; set; }= new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; } //// another way , may be  public IEnumerable<Category>? Categories { get; set; }
    }
}
