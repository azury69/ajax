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

    [Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult Create()
    {
        // Pass existing tickets to the view when rendering the create page
        var tickets = _context.Ticket.ToList();
        ViewBag.Tickets = tickets; // Use ViewBag to send the tickets to the view
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Ticket ticket)
    {
        if (ModelState.IsValid)
        {
            // Add new ticket to the database
            _context.Ticket.Add(ticket);
            _context.SaveChanges();

            // Return the ticket list again to update the bottom of the list.
            var tickets = _context.Ticket.ToList();
            ViewBag.Tickets = tickets;

            // Return the ticket details as a partial view (we'll use this in the front-end to update dynamically)
            return PartialView("_TicketDetails", ticket); // Send only the ticket for rendering
        }

        return BadRequest("Error creating ticket");
    }


    [Authorize(Roles = "Admin,User,SuperAdmin")]
    public IActionResult Index()
    {
        var tickets = _context.Ticket.ToList();
        return View(tickets);
    }

    [Authorize(Roles = "Admin,User,SuperAdmin")]
    public IActionResult Details(int id)
    {
        var ticket = _context.Ticket.FirstOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }
        return View(ticket);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult _TicketDetails(int id)
    {
        var ticket = _context.Ticket.FirstOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }
        return PartialView("_TicketDetails", ticket);
    }
}
