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

        public void insertBook(string title, string author, int publicationYear)
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
                insertCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public List<(string Title, int publicationYear)> selectByAuthor(string author)
        {
            List<(string Title, int publicationYear)> resultList = new List<(string Title, int publicationYear)>();
            string connectionString = "Data Source=SqliteDB.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT Title, PublicationYear FROM Books WHERE Author = $author"; //command parameter
                selectCmd.Parameters.AddWithValue("$author", author);
                using (var reader = selectCmd.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        resultList.Add((reader.GetString(0), reader.GetInt32(1)));
                    }
                }

                connection.Close();
            }

            return resultList;
        }
    }
