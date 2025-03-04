using makingticket.Data;
using makingticket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class TicketController : Controller
{
    private readonly ApplicationDbContext _context;

    public TicketController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Create Ticket Form
    [Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Create Ticket
    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            _context.Ticket.Add(ticket);
            _context.SaveChanges();

            // After saving, return the updated list of tickets
            var tickets = _context.Ticket.ToList();
            return PartialView("_TicketList", tickets); // Return updated ticket list
        }

        return BadRequest("Error creating ticket.");
    }

    // GET: Ticket List
    [Authorize(Roles = "Admin,User,SuperAdmin")]
    public IActionResult Index()
    {
        var tickets = _context.Ticket.ToList();
        return View(tickets);
    }

    // GET: Ticket Details by ID
    [Authorize(Roles = "Admin,User,SuperAdmin")]
    public IActionResult Details(int id)
    {
        var ticket = _context.Ticket.FirstOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }
        return PartialView("_TicketDetails", ticket); // Return ticket details as a partial view
    }
}
