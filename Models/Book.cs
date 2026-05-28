namespace HelloWorld.Models;
public class Book
{
    public int Id { get; set; }
    public string Title{get; set;} = "";
    public string Author{get; set;} = "";
    public string Description{get; set;} = "";
    public int PublicationYear{get; set;}
    public string Genre{ get; set; } = "Unknown";//Removed forced foreign key relation to separate table, just including Genre here as a string directly
    public string? CoverArt { get; set; } //Will likely just use the same placeholder image for all books at first, but this is scaffolding for raw functionality
    public string ReadingStatus { get; set; } = "Unread"; //Track whether the book is actively being read, if it's unread, or if it's already been completed.
    public decimal? Rating { get; set; } //Rating system.... 0-10.0 with 1 point of percision Rating wasn't nullable like it should've been based on our new Book schema!
    public DateTime DateAddedStamp { get; set; } //Set when the record was added to the database
    public DateTime LastUpdateStamp { get; set; } //Should be updated to the current date/time whenever changes are applied to the record. Looking into an EF property to handle this...

    public override string ToString()
    {
        return $"{Title}, {Author}, {PublicationYear}, Genre: {Genre}, Synopsis: {Description}, Status: {ReadingStatus}, Rating: {Rating}";
    }
}