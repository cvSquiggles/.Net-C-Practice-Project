using System.Security.Cryptography.X509Certificates;
using HelloWorld.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace HelloWorld.Data;

//DBContext class
public class EFSqliteContext : DbContext
{
    //I'm learning that my Library class and it's methods are redundant, since EntityFramework handles those things, I shouldn't need explicit methods. Going to try dropping them and see if actions work directly through the DBContext WebApp service.
    public EFSqliteContext(DbContextOptions<EFSqliteContext> options) : base(options) {} //My previous constructor used DbContextOptionsBuilder, but we don't need the builder, we just need the options themselves....we just extned the DbContext.base(options)

    private IConfiguration _config = new ConfigurationBuilder().AddJsonFile("AppSettings.json").Build();
   
    public DbSet<Book> Book { get; set; }
    public DbSet<Genre> Genre { get; set; }
    
    //Removed OnModelCreation and OnConfiguring, since these should be handled in the Program.cs where we AddDBContext.
}

//Removed the Library class entirely, since I'm learning it was entirely unnecessary. EF can handle Sqlite interactions without this extra level of obfuscation.