using System;
using System.Text.RegularExpressions;
using HelloWorld.Models;
using HelloWorld.Data;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace HelloWorld
{

    

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

            //SqliteDBTools dbTool = new SqliteDBTools();

            //dbTool.insertBook("Die Another Day", "James Bond", 1972);
            //dbTool.insertBook("Live and Let Die", "James Bond", 1984);

            //List<(string Title, string Author, int PublicationYear)> queryResult = dbTool.SelectAllBooks();

            //for(int i = 0; i < queryResult.Count(); i++)
            //{
            //    Console.WriteLine("*");
            //    Console.WriteLine(queryResult[i]);
            //}

            var library = new Library();
            library.InsertGenre("Film-Adaptation");

            //library.InsertBook("Die Another Day", "James Bond", "James Bond refuses to die on THIS day.", 1972, "Thriller");
            //library.InsertBook("Casino Royale", "James Bond", "James Bond goes to the casino hoping to win big.", 2007, "Film-Adaptation");

            List<Book> searchResult = library.SelectByAuthor("James Bond");

            for (int i = 0; i < searchResult.Count(); i++)
            {
                Console.WriteLine(searchResult[i]);
                Console.WriteLine("-----------------------");
            }
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