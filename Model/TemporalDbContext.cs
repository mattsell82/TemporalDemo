using Microsoft.EntityFrameworkCore;

namespace TemporalDemo.Model
{
    public class TemporalDbContext : DbContext
    {
        public TemporalDbContext(DbContextOptions<TemporalDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .ToTable("Book", c => c.IsTemporal());

            modelBuilder.Entity<Author>()
                .ToTable("Author", c => c.IsTemporal());

            modelBuilder.Entity<Book>()
                .Property(e => e.Id)
                .HasConversion(
                    v => v.Id,
                    v => new BookId(v)
                );

            modelBuilder.Entity<Author>()
                .Property(e => e.Id)
                .HasConversion(
                    v => v.Id,
                    v => new AuthorId(v)
                );



/*            modelBuilder.Entity<AuthorBook>()
                .HasKey(ab => new { ab.AuthorId, ab.BookId });

            modelBuilder.Entity<AuthorBook>()
                .HasOne(ab => ab.Author)
                .WithMany(a => a.AuthorBooks)
                .HasForeignKey(ab => ab.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Set delete behavior to restrict

            modelBuilder.Entity<AuthorBook>()
                .HasOne(ab => ab.Book)
                .WithMany(b => b.AuthorBooks)
                .HasForeignKey(ab => ab.BookId)
                .OnDelete(DeleteBehavior.Restrict);*/


            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithMany(b => b.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorBook",
                    ab => ab.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                    ab => ab.HasOne<Author>().WithMany().HasForeignKey("AuthorId")
                )
                .ToTable("AuthorBook", c => c.IsTemporal());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }

    }
}