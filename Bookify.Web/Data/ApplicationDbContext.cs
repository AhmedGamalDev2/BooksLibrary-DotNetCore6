
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace Bookify.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        #region DbSet
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCategory> BookCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Area> Areas { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("SerialNumber", schema: "shared")
                .StartsAt(1000001);
            builder.Entity<BookCopy>()
                           .Property(e => e.SerialNumber)
                           .HasDefaultValueSql("NEXT VALUE FOR shared.SerialNumber");
 
            builder.Entity<BookCategory>(e => e.HasKey(b => new {b.BookId,b.CategoryId})); // assign primary key for BookCategory entity 

            #region Apply Restrict on all onModelDelete instead of Cascade(the better to do(restrict all delete behavior) that in beginning of program or project)
            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);
            foreach(var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            #endregion

            base.OnModelCreating(builder);
        }

    }
}