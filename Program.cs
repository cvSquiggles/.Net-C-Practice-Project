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

            builder.Services.AddControllersWithViews(); //Enable MVC services and Razor view which I think is what is used for rendering?

            var app = builder.Build(); //Build the web app

            app.MapGet("/", () => "Hello World3!"); //Set root path to return the classic starter text message :)

            app.Logger.LogInformation("Application started successfully, does logging work?");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Books}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}