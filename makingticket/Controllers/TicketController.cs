using makingticket.Data;
using makingticket.Models;
using makingticket.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace makingticket.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var users = _context.Users
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName })
                .ToList();

            var viewModel = new TicketViewModel
            {
                UserList = users,
                Tickets = new List<Ticket>() // Start with no tickets initially
            };

            return View(viewModel);
        }

        // Action to get the ticket list, for loading it via AJAX
        public IActionResult GetTicketList()
        {
            var tickets = _context.Ticket.ToList(); // Fetch all tickets from DB

            return PartialView("_TicketList", tickets); // Return the ticket list as a partial view
        }

        // Partial View for Dynamic Ticket Forms
        // Partial View for Dynamic Ticket Forms
        public IActionResult GetCreatePartial(int index)
        {
            ViewData["Index"] = index; // Passing the index as ViewData
            var model = new TicketViewModel { UserList = _context.Users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName }).ToList() };
            return PartialView("_TicketCreate", model);
        }


        [HttpPost]
        public IActionResult SubmitAll(TicketViewModel model)
        {
            if (model.Tickets != null && model.Tickets.Count > 0)
            {
                foreach (var ticket in model.Tickets)
                {
                    _context.Ticket.Add(new Ticket
                    {
                        Name = ticket.Name,
                        Description = ticket.Description,
                        AssignedTo = ticket.AssignedTo,
                        StoryPoints = ticket.StoryPoints,
                        Status = ticket.Status,
                        CreatedAt = DateTime.Now
                    });
                }
                _context.SaveChanges();
            }

            var updatedTickets = _context.Ticket.OrderByDescending(t => t.CreatedAt).ToList();
            return PartialView("_TicketList", updatedTickets); // Return updated ticket list after submission
        }
        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")] // Only allow Admin or SuperAdmin to delete tickets
        public IActionResult DeleteTicket(int ticketId)
        {
            var ticket = _context.Ticket.Find(ticketId);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
                _context.SaveChanges();
            }

            // After deletion, return updated list of tickets
            var updatedTickets = _context.Ticket.OrderByDescending(t => t.CreatedAt).ToList();
            return PartialView("_TicketList", updatedTickets); // Return the updated ticket list
        }
        public IActionResult Details()
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "angularDetailsSpa", "browser", "index.html"),
                "text/html"
            );
        }


    }
}
