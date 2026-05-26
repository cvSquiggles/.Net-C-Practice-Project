

namespace HelloWorld.Models;
public class Book
{
    public int Id { get; set; }
    public string Title{get; set;} = "";
    public string Author{get; set;} = "";
    public string Description{get; set;} = "";
    public int PublicationYear{get; set;}
    public int GenreId{ get; set; }
    public Genre Genre { get; set; } = null!; //Navigation proprty for EF
    public override string ToString()
    {
        return $"{Title}, {Author}, {PublicationYear}, Genre: {Genre?.Name ?? "Unknown Genre"}, Synopsis: {Description}";
    }
}