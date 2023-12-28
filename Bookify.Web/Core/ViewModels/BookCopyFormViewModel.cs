
namespace Bookify.Web.Core.ViewModels
{
    public class BookCopyFormViewModel
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        [Display(Name = "Is available for rental?")]
        public bool IsAvailableForRental { get; set; }

        [Display(Name = "Edition Number"),
            Range(1,1000, ErrorMessage = Errors.InvalidRange)]
        public int EditionNumber { get; set; }

        public bool ShowRentalInput { get; set; }
    }
}