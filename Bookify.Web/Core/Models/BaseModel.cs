namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } // or = DateTime.MinValue;
        public DateTime? LastUpdatedOn { get; set; }

        public BaseModel()
        {
            CreatedOn = DateTime.Now;//initialize createdon
        }
    }
}
