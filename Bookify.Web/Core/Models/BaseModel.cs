namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } // or = DateTime.MinValue;
        public DateTime? LastUpdatedOn { get; set; }

        #region Who (andy user) did Create this table 
        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        #endregion

        #region Who (andy user) did LastUpdated this table
        public string? LastUpdatedById { get; set; }
        public ApplicationUser? LastUpdatedBy { get; set; }

        #endregion

        public BaseModel()
        {
            CreatedOn = DateTime.Now;//initialize createdon
        }
    }
}
