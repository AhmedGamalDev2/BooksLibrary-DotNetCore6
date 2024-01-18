namespace Bookify.Web.Core.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Governorate:BaseModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        ICollection<Area>? Areas { get; set; } = new List<Area>();
    }
}
