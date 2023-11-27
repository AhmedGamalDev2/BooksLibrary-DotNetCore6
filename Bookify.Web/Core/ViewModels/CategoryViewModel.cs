namespace Bookify.Web.Core.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }  
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }  
        public DateTime? LastUpdatedOn { get; set; }

    }
}
