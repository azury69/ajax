using makingticket.Data;
using makingticket.Models;
using makingticket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace makingticket.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create ticket page
        public IActionResult Create()
        {
            // Get the list of users for dropdown
            var users = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.UserName
                }).ToList();

            // Prepare view model with user list
            var viewModel = new TicketViewModel
            {
                UserList = users,
                Tickets = _context.Ticket.ToList()  // Pass initial tickets for the list
            };

            return View(viewModel);
        }

        // POST: Create ticket
        [HttpPost]
        public IActionResult Create(TicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add the new ticket to the database
                model.NewTicket.CreatedAt = DateTime.Now;
                _context.Ticket.Add(model.NewTicket);
                _context.SaveChanges();

                // Fetch the updated list of tickets after creating the new one
                var tickets = _context.Ticket.OrderByDescending(t => t.CreatedAt).ToList();

                // Return the updated ticket list as a partial view
                return PartialView("_TicketList", tickets);
            }

            // If the model state is invalid, return the form with errors
            return PartialView("_TicketCreate", model);
        }

        // GET: Get list of tickets for AJAX call
        [HttpGet]
        public IActionResult GetTickets()
        {
            var tickets = _context.Ticket
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            // Return the updated ticket list as a partial view
            return PartialView("_TicketList", tickets);
        }
    }
}
