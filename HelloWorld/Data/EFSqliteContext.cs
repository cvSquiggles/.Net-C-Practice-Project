using HelloWorld.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HelloWorld.Data;

//DBContext class
public class EFSqliteContext : DbContext
{
    public DbSet<Book> Book { get; set; }
    public DbSet<Genre> Genre { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
    => options.UseSqlite("Data Source=SqliteDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //EF Model Constraint example
        // modelBuilder.Entity<Book>(entity =>
        // {
        //     entity.Property(b => b.Title).HasMaxLength(200);
        // });

        //EF Model relationship
        modelBuilder.Entity<Book>().HasOne(b => b.Genre).WithMany().HasForeignKey(b => b.GenreId);
    }
}

public class Library
{
    //Insert book
    public int InsertBook(string title, string author, string description, int publicationYear, string genre)
    {
        using var context = new EFSqliteContext();

        var matchedGenre = context.Genre.FirstOrDefault(g => g.Name.ToLower() == genre.ToLower())
            ?? throw new KeyNotFoundException($"Genre '{genre}' not found.");

        var book = new Book
        {
            Title = title,
            Author = author,
            Description = description,
            PublicationYear = publicationYear,
            GenreId = matchedGenre != null ? matchedGenre.Id : -1
        };

        context.Book.Add(book);
        return context.SaveChanges();
    }

    public int InsertGenre(string name)
    {
        using var context = new EFSqliteContext();

        var genre = new Genre
        {
            Name = $"{name}"
        };

        context.Genre.Add(genre);
        return context.SaveChanges();
    }

    public List<Book> SelectByAuthor(string author)
    {
        using var context = new EFSqliteContext();
        var bookSearch = context.Book.Where(b => b.Author == author).ToList();

        if(bookSearch.Count == 0)
        {
            throw new KeyNotFoundException($"No books found by the author: {author}");
        }
        
        return bookSearch;
    }
}
