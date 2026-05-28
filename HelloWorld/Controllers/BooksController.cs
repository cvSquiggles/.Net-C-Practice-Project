using HelloWorld.Data;
using HelloWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Controllers;

//AspNetCore.Mvc.Controller class for Book objects.
public class BooksController : Controller
{
    private readonly EFSqliteContext _context;

    public BooksController(EFSqliteContext context)
    {
        _context = context;   
    }

    // Basic getter, just gets all books /Books
    public async Task<IActionResult> Index()
    {
        var books = await _context.Book.ToListAsync();
        return View(books);
    }

    // Get a book by a specific Id value /Books/{Id}
    public async Task<IActionResult> Details(int id)
    {
        var book = await _context.Book.FindAsync(id);
        return View(book);
    }
}