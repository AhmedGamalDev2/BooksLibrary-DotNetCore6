using System.Xml.Linq;

namespace Bookify.Web.Core.Models
{
    [Index(nameof(NationalId), IsUnique = true)]
    [Index(nameof(MobileNumber), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]

    public class Subscriber:BaseModel
    {

        public int Id { get; set; }
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
        [MaxLength(500)] //if you upload image on cloud service you will need MaxLength big size like 20000
        public string ImageUrl { get; set; } = null!;
        [MaxLength(500)] //if you upload image on cloud service you will need MaxLength big size like 20000
        public string ImageThumbnailUrl { get; set; } = null!;
        public int GovernorateId { get; set; }
        public Governorate? Governorate { get; set; }
        public int AreaId { get; set; }
        public Area? Area { get; set; }

        [MaxLength(500)]
        public string Address { get; set; } = null!;
        public bool IsBlackListed { get; set; }
    }
}
