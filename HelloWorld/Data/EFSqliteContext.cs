using System.Security.Cryptography.X509Certificates;
using HelloWorld.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HelloWorld.Data;

//DBContext class
public class EFSqliteContext : DbContext
{
    private IConfiguration _config = new ConfigurationBuilder().AddJsonFile("AppSettings.json").Build();
   
    public DbSet<Book> Book { get; set; }
    public DbSet<Genre> Genre { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
    => options.UseSqlite(_config.GetConnectionString("DefaultConnection"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Removed this foreign key EF relationship since the Book table absorbed the Genre table --modelBuilder.Entity<Book>().HasOne(b => b.Genre).WithMany().HasForeignKey(b => b.GenreId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class Library
{
    //Insert book
    public int InsertBook(string title, string author, string description, int publicationYear, string genre)
    {
        using var context = new EFSqliteContext();

        /*Removed genre check, since it's just stored directly in the book table as a string now --var matchedGenre = context.Genre.FirstOrDefault(g => g.Name.ToLower() == genre.ToLower())
            ?? throw new KeyNotFoundException($"Genre '{genre}' not found."); */

        var book = new Book
        {
            Title = title,
            Author = author,
            Description = description,
            PublicationYear = publicationYear,
            Genre = genre
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

    //Commented out since it's causing errors trying to build the web app, and genreId doesn't even exist anymore!
    // public int InsertBookWithGenreId(string title, string author, string description, int publicationYear, int genreId)
    // {
    //     using var context = new EFSqliteContext();

    //     var book = new Book
    //     {
    //         Title = title,
    //         Author = author,
    //         Description = description,
    //         PublicationYear = publicationYear,
    //         GenreId = genreId
    //     };

    //     var duplicateBook = context.Book.Where(b=> b.Title.ToLower() == title.ToLower() 
    //                                             && b.Author.ToLower() == author.ToLower()
    //                                             && b.PublicationYear == publicationYear).ToList();

    //     if (duplicateBook.Count() > 0)
    //     {
    //         Console.WriteLine("No new book created in the library. A book with this title, author, and publication year already exists.");
    //         return 0;
    //     }
    //     else
    //     {
    //         context.Book.Add(book);
    //         return context.SaveChanges();
    //     }
    // }

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

    public List<Genre> SelectAllGenres()
    {
        var genreSearch = new List<Genre>();
        try
        {
            using var context = new EFSqliteContext();
            genreSearch = context.Genre.ToList();

            if(genreSearch.Count == 0)
            {
                throw new KeyNotFoundException("The genre data table returned 0 records!");
            }
        }
        catch (Exception e)
        {
            Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": " + e);
        }
        return genreSearch;
    }

    public int SelectGenreIdByName(string name)
    {
        Genre targetGenre = new Genre();
        try
        {
            using var context = new EFSqliteContext();
            targetGenre = context.Genre.Where(g=> g.Name.ToLower().Equals(name.ToLower())).First();
            Console.WriteLine(name + " <-- | --> " + targetGenre.Name + " - " + targetGenre.Id);

            if(targetGenre != null)
            {
                return targetGenre.Id;
            }
        }
        catch (Exception e)
        {
            Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": " + e);
        }
        return -1;
    }

    public List<Book> SelectByAuthor(string author)
    {
        var bookSearch = new List<Book>();
        try
        {
            using var context = new EFSqliteContext();
            bookSearch = context.Book.Include(g=> g.Genre).Where(b => b.Author == author).ToList();

            if(bookSearch.Count == 0)
            {
                throw new KeyNotFoundException($"No books found by the author: {author}");
            }
            
        }
        catch (KeyNotFoundException e)
        {
            Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": " + e);
        }
        return bookSearch;
    }

    public List<Book> SelectAllBooks()
    {
        var bookSearch = new List<Book>();
        try
        {
            using var context = new EFSqliteContext();
            bookSearch = context.Book.Include(g=> g.Genre).ToList();

            if(bookSearch.Count == 0)
            {
                throw new KeyNotFoundException("The book data table returned 0 records!");
            }
        }
        catch (Exception e)
        {
            Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": " + e);
        }
        return bookSearch;
    }

    public int DeleteBook(int id)
    {
        using var context = new EFSqliteContext();

        var rowsAffected = context.Book.Where(b=> b.Id == id).ExecuteDelete();

        return rowsAffected;
    }

    public int DeleteGenre(int id)
    {
        using var context = new EFSqliteContext();

        var rowsAffected = context.Genre.Where(g=> g.Id == id).ExecuteDelete();

        return rowsAffected;
    }
}
