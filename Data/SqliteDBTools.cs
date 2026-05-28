using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace HelloWorld.Data;

//Entity framework override for Sqlite
//protected override void OnConfiguring(DbContextOptionsBuilder options)
//    => options.UseSqlite("Data Source=customer.db");

public class SqliteDBTools(IConfiguration config)
{
    private string connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Default connection string in AppSettings.json NOT FOUND.");

    //Currently only creates an empty table with a primary key and no fields
    public void createTable(string tableName)
    {
        //Define table connection

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
            using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Book (Title, Author, PublicationYear) VALUES ($title, $author, $publicationYear)";
            insertCmd.Parameters.AddWithValue("$title", title);
            insertCmd.Parameters.AddWithValue("$author", author);
            insertCmd.Parameters.AddWithValue("$publicationYear", publicationYear);
            int rowsAdded = insertCmd.ExecuteNonQuery();

            connection.Close();
            return rowsAdded;
        }
    }

    public int insertGenre(int genreId, string name)
    {
            using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Genre (Id, Name) VALUES ($genreId, $name)";
            insertCmd.Parameters.AddWithValue("$genreId", genreId);
            insertCmd.Parameters.AddWithValue("$name", name);
            int rowsAdded = insertCmd.ExecuteNonQuery();

            connection.Close();
            return rowsAdded;
        }
    }

    //Type generic select statement method
    public List<T> ExecuteQuery<T>(string query, Func<SqliteDataReader, T> mapper, Dictionary<string, object>? parameters = null)
    {
        List<T> resultList = new List<T>();

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
            "SELECT Title, PublicationYear FROM Book WHERE Author = $author",
            reader => (reader.GetString(reader.GetOrdinal("Title")), reader.GetInt32(reader.GetOrdinal("PublicationYear"))),
            new Dictionary<string, object> { {"$author", author} }
        );
    }

    public List<(string Title, string Author, int PublicationYear)> SelectAllBooks()
    {
        return ExecuteQuery(
            "SELECT * FROM Book",
            reader => (reader.GetString(reader.GetOrdinal("Title")), reader.GetString(reader.GetOrdinal("Author")), reader.GetInt32(reader.GetOrdinal("PublicationYear")))
        );
    }
}
