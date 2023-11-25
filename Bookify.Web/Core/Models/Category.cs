
namespace Bookify.Web.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; //or =null! (not null) // assign default value  
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } // or = DateTime.MinValue;
        public DateTime? LastUpdatedOn { get; set; }

        public Category()
        {
            CreatedOn = DateTime.Now;//initialize createdon
        }
    }
}
