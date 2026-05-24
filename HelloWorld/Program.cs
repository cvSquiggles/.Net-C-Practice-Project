using System;
using System.Text.RegularExpressions;

namespace HelloWorld
{

    public class Book
    {
        public string Title{get; set;}

        public string Author{get; set;}

        public int PublicationYear{get; set;} 

        public Book()
        {
            if(Title == null)
            {
                Title = "";
            }
            if(Author == null)
            {
                Author = "";
            }
        }
    }

    internal class Program
    {
        //static int AccessibleInt = 7;

        static void Main(string[] args)
        {
            Book firstBook = new Book()
            {
                Title = "Die Another Day",
                Author = "James Bond",
                PublicationYear = 1972
            };
            
            Console.WriteLine(firstBook.Author);
        }
    }
}



//----------------------------------
// using Microsoft.Data.Sqlite;

// //Define table connection
// string connectionString = "Data Source=SqliteDB.db";

// using (var connection = new SqliteConnection(connectionString))
// {
//     connection.Open();
//     //just one simple table for now, probably do something more interesting like author table separate with foreign key in book table
//     //var createTableBooks = connection.CreateCommand();
//     //createTableBooks.CommandText = "CREATE TABLE IF NOT EXISTS Books (Id INTEGER PRIMARY KEY, Title TEXT, Author TEXT, PublicationYear INTEGER)";
//     //createTableBooks.ExecuteNonQuery();

//     //Populate table with some entries
//     // var insertCmd = connection.CreateCommand();
//     // insertCmd.CommandText = "INSERT INTO Books (Title, Author, PublicationYear) VALUES ($title, $author, $publicationYear)";
//     // insertCmd.Parameters.AddWithValue("$title", "Project Hail Mary");
//     // insertCmd.Parameters.AddWithValue("$author", "Andy Weir");
//     // insertCmd.Parameters.AddWithValue("$publicationYear", "2020");
//     // insertCmd.ExecuteNonQuery();

//     //Select statement
//     var selectCmd = connection.CreateCommand();
//     selectCmd.CommandText = "SELECT Title FROM Books";
//     using (var reader = selectCmd.ExecuteReader())
//     {
//         while (reader.Read())
//         {
//             var title = reader.GetString(0);
//             Console.WriteLine($"Found book titled: {title}");
//         }
//     }

//     connection.Close();
// }