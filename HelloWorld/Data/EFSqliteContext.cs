using System.Security.Cryptography.X509Certificates;
using HelloWorld.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.Data;

//DBContext class
public class EFSqliteContext : DbContext
{
    private IConfiguration _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json").Build();
   
    public DbSet<Book> Book { get; set; }
    public DbSet<Genre> Genre { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
    => options.UseSqlite(_config.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //EF Model Constraint example
        // modelBuilder.Entity<Book>(entity =>
        // {
        //     entity.Property(b => b.Title).HasMaxLength(200);
        // });

        //EF Model relationship
        modelBuilder.Entity<Book>().HasOne(b => b.Genre).WithMany().HasForeignKey(b => b.GenreId).OnDelete(DeleteBehavior.Restrict);
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

        var duplicateBook = context.Book.Where(b=> b.Title.ToLower() == title.ToLower() 
                                                && b.Author.ToLower() == author.ToLower()
                                                && b.PublicationYear == publicationYear).ToList();

        if (duplicateBook.Count() > 0)
        {
            Console.WriteLine("No new book created in the library. A book with this title, author, and publication year already exists.");
            return 0;
        }
        else
        {
            context.Book.Add(book);
            return context.SaveChanges();
        }
    }

    public int InsertGenre(string name)
    {
        using var context = new EFSqliteContext();

        var genre = new Genre
        {
            Name = $"{name}"
        };

        var duplicateGenre = context.Genre.Where(g=> g.Name.ToLower() == name.ToLower()).ToList();

        if (duplicateGenre.Count() > 0)
        {
            Console.WriteLine("No new Genre created in the library. A Genre by this name already exists.");
            return 0;
        }
        else
        {
            context.Genre.Add(genre);
            return context.SaveChanges();
        }
    }

    public List<Book> SelectByAuthor(string author)
    {
        using var context = new EFSqliteContext();
        var bookSearch = context.Book.Include(g=> g.Genre).Where(b => b.Author == author).ToList();

        if(bookSearch.Count == 0)
        {
            throw new KeyNotFoundException($"No books found by the author: {author}");
        }
        
        return bookSearch;
    }

    public int DeleteBook(int id)
    {
        using var context = new EFSqliteContext();

        var rowsAffected = context.Book.Where(b=> b.Id == id).ExecuteDelete();

        return rowsAffected;
    }
}
