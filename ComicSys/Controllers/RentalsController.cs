using ComicSys.Data;
using ComicSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;


public class RentalsController : Controller
{
    private readonly ComicSystemContext _context;

    public RentalsController(ComicSystemContext context)
    {
        _context = context;
    }

    public IActionResult Rent(int customerId)
    {
        var books = _context.ComicBooks.ToList();
        ViewBag.CustomerId = customerId;
        return View(books);
    }

    [HttpPost]
    public async Task<IActionResult> Rent(int customerId, List<int> comicBookIds, List<int> quantities)
    {
        var rental = new Rental
        {
            CustomerID = customerId,
            RentalDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(7)
        };
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        for (int i = 0; i < comicBookIds.Count; i++)
        {
            var rentalDetail = new RentalDetail
            {
                RentalID = rental.RentalID,
                ComicBookID = comicBookIds[i],
                Quantity = quantities[i]
            };
            _context.RentalDetails.Add(rentalDetail);
        }
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "ComicBooks");
    }
    public IActionResult RentalReport(DateTime startDate, DateTime endDate)
{
    var rentals = from r in _context.Rentals
                  join rd in _context.RentalDetails on r.RentalID equals rd.RentalID
                  join c in _context.ComicBooks on rd.ComicBookID equals c.ComicBookID
                  join cu in _context.Customers on r.CustomerID equals cu.CustomerID
                  where r.RentalDate >= startDate && r.RentalDate <= endDate
                  select new RentalReportViewModel
                  {
                      BookName = c.Title,
                      RentalDate = r.RentalDate,
                      ReturnDate = r.ReturnDate,
                      CustomerName = cu.FullName,
                      Quantity = rd.Quantity
                  };

    return View(rentals.ToList());
}

    
}

internal class RentalReportViewModel
{
    public string BookName { get; set; }
    public DateTime RentalDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public object CustomerName { get; set; }
    public int Quantity { get; set; }
}