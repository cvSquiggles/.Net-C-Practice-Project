using System.IO.MemoryMappedFiles;
using HelloWorld.Models;
using Microsoft.Data.Sqlite;

namespace HelloWorld.Data;


public class SqliteDBTools()
{
    //Currently only creates an empty table with a primary key and no fields
    public void createTable(string tableName)
    {
        //Define table connection
        string connectionString = "Data Source=SqliteDB.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var createTableBooks = connection.CreateCommand();
            createTableBooks.CommandText = "CREATE TABLE IF NOT EXISTS " + tableName + " (Id INTEGER PRIMARY KEY)"; //string concat manual
            createTableBooks.ExecuteNonQuery();

            connection.Close();
        }
    }

    public int insertBook(string title, string author, int publicationYear)
    {
        string connectionString = "Data Source=SqliteDB.db";
            using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Books (Title, Author, PublicationYear) VALUES ($title, $author, $publicationYear)";
            insertCmd.Parameters.AddWithValue("$title", title);
            insertCmd.Parameters.AddWithValue("$author", author);
            insertCmd.Parameters.AddWithValue("$publicationYear", publicationYear);
            int rowsAdded = insertCmd.ExecuteNonQuery();

            connection.Close();
            return rowsAdded;
        }
    }

    //Type generic select statement method
    public List<T> ExecuteQuery<T>(string query, Func<SqliteDataReader, T> mapper, Dictionary<string, object>? parameters = null)
    {
        List<T> resultList = new List<T>();
        string connectionString = "Data Source=SqliteDB.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;

            if (parameters != null)
            {
                foreach(var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    resultList.Add(mapper(reader));
                }
            }

            connection.Close();
            return resultList;
        }
    }

    public List<(string Title, int PublicationYear)> SelectByAuthor(string author)
    {
        return ExecuteQuery(
            "SELECT Title, PublicationYear FROM Books WHERE Author = $author",
            reader => (reader.GetString(reader.GetOrdinal("Title")), reader.GetInt32(reader.GetOrdinal("PublicationYear"))),
            new Dictionary<string, object> { {"$author", author} }
        );
    }

    public List<(string Title, string Author, int PublicationYear)> SelectAllBooks()
    {
        return ExecuteQuery(
            "SELECT * FROM Books",
            reader => (reader.GetString(reader.GetOrdinal("Title")), reader.GetString(reader.GetOrdinal("Author")), reader.GetInt32(reader.GetOrdinal("PublicationYear")))
        );
    }
}
