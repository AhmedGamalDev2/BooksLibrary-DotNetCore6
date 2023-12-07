using Bookify.Web.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; } //for edit
        [MaxLength(500, ErrorMessage = Errors.MaxLenth)]
        [Remote("AllowBook", "Books", AdditionalFields = "Id,AuthorId", ErrorMessage = Errors.DublicatedBook)]
        public string Title { get; set; } = null!;
        [Display(Name ="Author")]
        [Remote("AllowBook", "Books", AdditionalFields = "Id,Title", ErrorMessage = Errors.DublicatedBook)]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; } // another way , may be  public IEnumerable<Author>? Authors { get; set; }
        [MaxLength(200, ErrorMessage = Errors.MaxLenth)]
        public string Publisher { get; set; } = null!;
        [Display(Name = "Publishing Date")]
        [AssertThat("PublishingDate <= Today()",ErrorMessage = Errors.NotAllowFutureDates)]
        public DateTime PublishingDate { get; set; } = DateTime.Now;
         
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
        [MaxLength(50, ErrorMessage = Errors.MaxLenth)]
        public string Hall { get; set; } = null!;
        [Display(Name = "Is available for rental?")]
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        [Display(Name ="Categories")]
        public IList<int> SelectedCategoryIds { get; set; }= new List<int>();
        public IEnumerable<SelectListItem>? Categories { get; set; } //// another way , may be  public IEnumerable<Category>? Categories { get; set; }
    }
}
