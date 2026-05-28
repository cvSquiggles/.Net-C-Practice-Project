using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelloWorld.Migrations
{
    /// <inheritdoc />
    public partial class FixBookSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Rename the existing tables since they'll be deleted but we need to copy the Genre.Name into the new Book table first.
            migrationBuilder.Sql(@"ALTER TABLE Book RENAME TO Book_Temp;");
            migrationBuilder.Sql(@"ALTER TABLE Genre RENAME TO Genre_Temp;");

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PublicationYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    CoverArt = table.Column<string>(type: "TEXT", nullable: true),
                    ReadingStatus = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<decimal>(type: "TEXT", nullable: true),
                    DateAddedStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdateStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
            });

            /*Now that we've created our new Book table, we need to insert the records from the old book table, but populate the Genre field with the Genre.Name string value.
            * Default the time stamp fields, and set Genre to 'unknown' if for some reason we don't find a mapping.*/
            migrationBuilder.Sql(@"
                INSERT INTO Book (Id, Title, Author, Description, PublicationYear, Genre, ReadingStatus, DateAddedStamp, LastUpdateStamp)
                SELECT
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Description,
                    b.PublicationYear,
                    COALESCE(g.Name, 'Unknown'),
                    'Unknown',
                    datetime('now'),
                    datetime('now')
                FROM Book_Temp b
                LEFT JOIN Genre_Temp g On b.GenreId = g.Id;
            ");

            //After this, we need to drop the old tables!
            migrationBuilder.Sql(@"DROP TABLE Book_Temp;");
            migrationBuilder.Sql(@"DROP TABLE Genre_Temp;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Genre");
        }
    }
}
