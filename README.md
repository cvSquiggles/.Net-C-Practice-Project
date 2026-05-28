# About .Net-C-Practice-Library-Project
This is a ASP .NET Core Web application with Model View Controller, Entity Framework Core, and SQLite databasing. I'm building this to refresh myself on C#, and learn about Entity Framework, Mvc, ASP .NET Core, and other areas of development related to these tools that I'm less familiar with.

The long term goal for the application is to allow User creation with username/password/email and authentication (maybe google authentication integration?). From there each user could add books to the shared library, as well as
maintain their own "personal" library including books they want to read, are reading, or have read. In addition, cover art will be included in the book view details page, and users will be able to give a rating to each book they've read.

Longer term goals would probably include personal and general statistics. Which genre of books are read most by a user, and which are read most by all users combined, things like that.

Another fun feature to add might be to include a "Report" feature, where users can report a book that they think is fake, or has incorrect information. There's already some minor duplicate book prevention (removed by WebApp.DbContext service conversion, but will be added back)
but things like a book being addded with a PublicationYear value when the book was never published for example. The report feature could either queue up reports in a View only accessible to a "Super User" to handle within the application entirely, or maybe in addition,
the reports could kick an email out to an "Super User" email address.


## Project Structure

```
HelloWorld/
├── Controllers/
│   ├── BooksController.cs
│   └── GenreController.cs --deprecated
├── Data/
│   ├── EFSqliteContext.cs --WebApp.DBContext Entity Framework
│   ├── JSONDataTools.cs --Used to import large json dataset of books and genres, may retool into API functions later
│   └── SqliteDBTools.cs --deprecated
├── Import/
│   └── JSON/
│       ├── books.json --source json for books
│       └── genres.json --source json for genres
├── Logs/
│   ├──                 --Serilog application logging lands here
├── Migrations/
│   ├──                --EF migration files
├── Models/
│   ├── Book.cs
│   ├── BookDTO.cs      --deprecated
│   └── Genre.cs        --deprecated
├── Views/
│   ├── Books/
│   ├── Genre/          --deprecated
│   ├── Shared/              --general shared layout
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   └── css/
│       └── site.css         --site-wide css file
├── appsettings.json
├── compose.debug.yaml
├── compose.yaml
├── Dockerfile
├── HelloWorld.csproj
├── Program.cs
├── README.md
└── SQLiteDB.db
```

## Book Schema

```sql
Book
-----
Id              INTEGER PRIMARY KEY
Title           TEXT NOT NULL
Author          TEXT NOT NULL
Description     TEXT
PublicationYear INTEGER
Genre           TEXT           -- plain string, no FK
CoverArt        TEXT           -- optional book cover URL, not implemented yet in app, just db table
ReadingStatus   TEXT           -- 'Unread' | 'Reading' | 'Finished'
Rating          DECIMAL        -- 0-10, nullable
DateAddedStamp  TEXT           -- ISO datetime
LastUpdateStamp TEXT           -- ISO datetime
```

## Running locally

```bash
# Install .NET 10 from https://dotnet.microsoft.com/download

cd LibraryApp
dotnet run

# Open http://localhost:5000
```

### This should be hosted via Railway.app through the repo! (will add URL once I have one)