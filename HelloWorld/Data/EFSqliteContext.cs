using HelloWorld.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data;

//DBContext class
public class EFSqliteContext : DbContext
{
    public DbSet<Book> Books {get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options) 
    => options.UseSqlite("Data Source=SqliteDB.db");
}

public class Library
{
    //Insert book
    public int InsertBook(string title, string author, int publicationYear)
    {
        using var context = new EFSqliteContext();

        var book = new Book
        {
            Title = "Count of Monte Cristo",
            Author = "Alexandre Dumas",
            PublicationYear = 1846
        };

        context.Books.Add(book);
        return context.SaveChanges();
    }

    public List<Book> SelectByAuthor(string author)
    {
        using var context = new EFSqliteContext();
        var bookSearch = context.Books.Where(b => b.Author == author).ToList();

        if(bookSearch.Count == 0)
        {
            throw new KeyNotFoundException($"No books found by the author: {author}");
        }
        
        return bookSearch;
    }
}
