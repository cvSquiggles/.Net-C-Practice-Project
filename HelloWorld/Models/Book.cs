namespace HelloWorld.Models;
public class Book
{
    public int Id { get; set; }
    public string Title{get; set;} = "";
    public string Author{get; set;} = "";
    public string Description{get; set;} = "";
    public int PublicationYear{get; set;}
    public int Genre{ get; set; } //Removed forced foreign key relation to separate table, just including Genre here as a string directly
    public string? CoverArt { get; set; } //Will likely just use the same placeholder image for all books at first, but this is scaffolding for raw functionality
    public string ReadingStatus { get; set; } = "Unread"; //Track whether the book is actively being read, if it's unread, or if it's already been completed.
    public decimal Rating { get; set; } //Rating system.... 0-10.0 with 1 point of percision

    public override string ToString()
    {
        return $"{Title}, {Author}, {PublicationYear}, Genre: {Genre}, Synopsis: {Description}, Status: {ReadingStatus}, Rating: {Rating}";
    }
}