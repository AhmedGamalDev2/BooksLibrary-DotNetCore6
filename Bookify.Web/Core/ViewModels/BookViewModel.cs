namespace Bookify.Web.Core.ViewModels
{
    public class BookViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        #region Author Details
        /*when mapping between Author in BookViewModel and Author in Book
       * =>Author in BookViewModel is string type and Author in Book is Author type
       * the default and the expected that the two Author not map
       * but here => because Author in Book is Author type(Navigation property) 
       * ,when mapping: it convert to string(Author in BookViewModel) automatically(Author.ToString())
       * so when map we must write ForMember(destBookViewModel => destBookViewModel.Author, options => options.MapFrom(sourceBook => sourceBook.Author!.Name)); in mapping profile
       */
        #endregion
        public string Author { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageThumbnailUrl { get; set; }
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public IEnumerable<string> Categories { get; set; } = new List<string>();
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
