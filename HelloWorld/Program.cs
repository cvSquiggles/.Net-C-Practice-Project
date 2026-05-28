using System;
using System.Text.RegularExpressions;
using HelloWorld.Models;
using HelloWorld.Data;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Text.Json;
using HellowWorld.Data;

namespace HelloWorld
{

    

    internal class Program
    {

        static void Main(string[] args)
        {
            //Create Serilog up front for debug and general application logging
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("Logs/error-log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args); //Create the ASPNetCore web application object
            builder.Host.UseSerilog(); //Declare Serilog as the logging provider in the Web Application Host

            //Add EFSqlite context to the web app as a service
            builder.Services.AddDbContext<EFSqliteContext>(options=> options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build(); //Build the web app

            app.MapGet("/", () => "Hello World!"); //Set root path to return the classic starter text message :)

            app.Logger.LogInformation("Application started successfully, does logging work?");

            //Testing the EFSqlite context through the WebApp service, using Genre to test since I haven't fixed the Book database table yet, but Genre insert should still be intact
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<EFSqliteContext>();

                var newGenre = new Genre
                {
                    Name = "Test-Genre"
                };

                context.Genre.Add(newGenre);
                if (context.SaveChanges() > 0)
                {
                    app.Logger.LogInformation("Genre added to the database successfully.");
                }
            }

            app.Run();
        }
    }
}