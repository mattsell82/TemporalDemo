using System.ComponentModel.DataAnnotations;

namespace TemporalDemo.Model
{
    public class Book(string title)
    {
        [Key]
        public BookId Id { get; set; } = new(Guid.NewGuid());

        public string Title { get; set; } = title;

        public List<Author> Authors { get; set; } = new List<Author>();

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }

    public class Author(string firstName, string lastName)
    {
        [Key]
        public AuthorId Id { get; set; } = new(Guid.NewGuid());

        public string FirstName { get; set; } = firstName;

        public string LastName { get; set; } = lastName;
        public List<Book> Books { get; set; } = new List<Book>();

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }



    public record AuthorId(Guid Id);

    public record BookId(Guid Id);




}
