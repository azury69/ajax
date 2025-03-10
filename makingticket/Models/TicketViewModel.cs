﻿using makingticket.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class TicketViewModel
{
    public Ticket NewTicket { get; set; } = new Ticket();  // Form ticket
    public List<Ticket> Tickets { get; set; } = new List<Ticket>(); // Ticket list
    public List<SelectListItem> UserList { get; set; } = new List<SelectListItem>(); // Dropdown users

    public TicketPriority SelectedPriority { get; set; } // To select priority from dropdown
    public TicketStatus SelectedStatus { get; set; } // To select status from dropdown
    // Add other fields like labels, attachments, etc.
}
