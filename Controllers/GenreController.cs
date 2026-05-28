using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers;

//This is a temporary AspNetCore.Mvc.Controller class to test the MapControllerRoute functionality in Program.cs, easier to setup then doing all the work to fix the books table and THEN testing it.
public class GenreController : Controller
{
    private readonly EFSqliteContext _context;

    public GenreController(EFSqliteContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var genres = await _context.Genre.ToListAsync();
        return View(genres);
    }
}
