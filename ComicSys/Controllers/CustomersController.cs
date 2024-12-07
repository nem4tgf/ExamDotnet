using ComicSys.Data;
using ComicSys.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ComicSys.Controllers
{
   public class CustomersController : Controller
{
    private readonly ComicSystemContext _context;

    public CustomersController(ComicSystemContext context)
    {
        _context = context;
    }

    // Register a new customer
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([Bind("Fullname, PhoneNumber")] Customer customer)
    {
        if (ModelState.IsValid)
        {
            customer.RegistrationDate = DateTime.Now;
            _context.Add(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ComicBooks");
        }
        return View(customer);
    }
}

}
