using makingticket.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace makingticket.Controllers;

public static class TicketViewModelEndpoints
{
    public static void MapTicketViewModelEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TicketViewModel").WithTags(nameof(TicketViewModel));

        // 1. Get all tickets (public)
        group.MapGet("/", (ApplicationDbContext context) =>
        {
            var tickets = context.Ticket.ToList(); // Fetch all tickets
            return Results.Ok(tickets); // Return the list of tickets
        })
        .WithName("GetAllTicketViewModels")
        .WithOpenApi();

        // 2. Get a specific ticket by ID (public)
        group.MapGet("/{id}", async (int id, ApplicationDbContext context) =>
        {
            var ticket = await context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return Results.NotFound(); // If ticket not found, return 404
            }
            return Results.Ok(ticket); // Return the ticket details
        })
        .WithName("GetTicketViewModelById")
        .WithOpenApi();
    }
}

