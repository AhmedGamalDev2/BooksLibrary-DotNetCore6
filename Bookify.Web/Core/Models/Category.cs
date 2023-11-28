
namespace Bookify.Web.Core.Models
{
    [Index(nameof(Name),IsUnique =true)]
    public class Category: BaseModel
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; //or =null! (not null) // assign default value  
        public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();

    }
}
