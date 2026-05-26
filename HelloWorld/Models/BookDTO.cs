

namespace HelloWorld.Models;
public class BookDTO
{
    public int Id { get; set; }
    public string Title{get; set;} = "";
    public string Author{get; set;} = "";
    public string Description{get; set;} = "";
    public int PublicationYear{get; set;}
    public string Genre{ get; set; } = "Unknown";
    public override string ToString()
    {
        return $"{Title}, {Author}, {PublicationYear}, Genre: {Genre}, Synopsis: {Description}";
    }
}