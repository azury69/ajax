using makingticket.Data;
using makingticket.Models;
using makingticket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.UserName
                }).ToList();

            var viewModel = new TicketViewModel
            {
                UserList = users
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(TicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.NewTicket.CreatedAt = DateTime.Now;
                _context.Ticket.Add(model.NewTicket);
                _context.SaveChanges();

                // Fetch the updated list of tickets
                var tickets = _context.Ticket.ToList();

                // Return the updated ticket list as a partial view
                return PartialView("_TicketList", tickets);
            }

            // If model state is invalid, return the form with errors
            return PartialView("_TicketCreate", model);
        }


    }
}
