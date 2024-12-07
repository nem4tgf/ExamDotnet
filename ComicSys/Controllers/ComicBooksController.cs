using ComicSys.Data;
using ComicSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace ComicSys.Controllers
{
    public class ComicBooksController : Controller
{
    private readonly ComicSystemContext _context;

    public ComicBooksController(ComicSystemContext context)
    {
        _context = context;
    }

    // CREATE: Get & Post
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Title, Author, PricePerDay")] ComicBook comicBook)
    {
        if (ModelState.IsValid)
        {
            _context.Add(comicBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(comicBook);
    }

    // READ: List all ComicBooks
    public async Task<IActionResult> Index()
    {
        return View(await _context.ComicBooks.ToListAsync());
    }

    // UPDATE: Get & Post
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comicBook = await _context.ComicBooks.FindAsync(id);
        if (comicBook == null)
        {
            return NotFound();
        }
        return View(comicBook);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("ComicBookID, Title, Author, PricePerDay")] ComicBook comicBook)
    {
        if (id != comicBook.ComicBookID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comicBook);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ComicBooks.Any(e => e.ComicBookID == comicBook.ComicBookID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(comicBook);
    }

    // DELETE: Delete ComicBook
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var comicBook = await _context.ComicBooks
            .FirstOrDefaultAsync(m => m.ComicBookID == id);
        if (comicBook == null)
        {
            return NotFound();
        }

        _context.ComicBooks.Remove(comicBook);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

}
