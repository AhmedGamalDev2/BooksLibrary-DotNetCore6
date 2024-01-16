namespace Bookify.Web.Core.Models
{
    public class BaseModel
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; } // or = DateTime.MinValue;
        public DateTime? LastUpdatedOn { get; set; }

        #region Who did created and lastupated  this user 
        /* do not write navigation property because it occur problem 
         * ,only write string? CreatedById and string? LastUpdatedById
         * Self Relationship
         */
        public string? CreatedById { get; set; }

        public string? LastUpdatedById { get; set; }
        #endregion
        public BaseModel()
        {
            CreatedOn = DateTime.Now;//initialize createdon
        }
    }
}
