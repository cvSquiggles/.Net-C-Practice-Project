namespace HellowWorld.Data;

using System.Text.Json;
using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.Extensions.Logging;
using Serilog;

public static class JSONDataTools
{
    public static void ParseAndUploadGenres(string json)
    {
        try
        {
            IEnumerable<Genre>? newGenres = JsonSerializer.Deserialize<IEnumerable<Genre>>(json);

            if(newGenres != null && newGenres.Count() > 0)
            {
                var library = new Library();
                foreach(Genre x in newGenres)
                {
                    Console.WriteLine(x.Name);
                    if(library.InsertGenre(x.Name) > 0)
                    {
                        Console.WriteLine("Genre " + x + " was added to the database successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Genre " + x + " was not able to be added to the database. See error log for details.");
                    }
                }
            }
            else
            {
                Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": Failed to parse new Genres from the provided JSON file.");
            }
        }
        catch(Exception e)
        {
            Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": "+ e);
        }
    }

    public static int ParseAndUploadBooks(string json)
    {
        int booksAdded = 0;
        try
        {
            IEnumerable<BookDTO>? newBooks = JsonSerializer.Deserialize<IEnumerable<BookDTO>>(json);

            if(newBooks != null && newBooks.Count() > 0)
            {
                var library = new Library();

                List<Book> booksWithGenre = newBooks.Select(dto=> new Book
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    Description = dto.Description,
                    PublicationYear = dto.PublicationYear,
                    GenreId = library.SelectGenreIdByName(dto.Genre)
                }).ToList();

                if(booksWithGenre.Count() > 0)
                {
                    foreach(Book x in booksWithGenre)
                    {
                        if(library.InsertBookWithGenreId(x.Title, x.Author, x.Description, x.PublicationYear, x.GenreId) > 0)
                        {
                            booksAdded++;
                        }
                    }
                }
            }
            else
            {
                Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": Failed to parse new Genres from the provided JSON file.");
            }
        }
        catch(Exception e)
        {
            Log.Error(DateTime.Now.ToShortDateString().Replace("/", "-") + ": "+ e);
        }

        return booksAdded;
    }
}