using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web.Core.ViewModels
{
    public class SubscriberFormViewModel
    {

        public int? Id { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        [MaxLength(20)]
        public string NationalId { get; set; } = null!;
        [MaxLength(11)]
        public string MobileNumber { get; set; } = null!;
        public bool HasWhatsApp { get; set; }
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public IFormFile PersonalImage { get; set; } = null!;
        [MaxLength(500)] //if you upload image on cloud service you will need MaxLength big size like 20000
        public string? ImageUrl { get; set; } = null!;
        [MaxLength(500)] //if you upload image on cloud service you will need MaxLength big size like 20000
        public string? ImageThumbnailUrl { get; set; } = null!;
        public int SelectedGovernorateId { get; set; }
        public IEnumerable<SelectListItem>? GovernorateNameIds { get; set; }//fill dropdown
         
        public int SelectedAreaId { get; set; }
        public IEnumerable<SelectListItem>? AreaNameIds { get; set; } //fill dropdown

        [MaxLength(500)]
        public string Address { get; set; } = null!;
        public bool IsBlackListed { get; set; }
    }
}
