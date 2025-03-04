namespace makingticket.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }  // User assigned to this ticket
        public int StoryPoints { get; set; }    // Fibonacci story points
        public string Status { get; set; }      // Status (Working/Completed)
        public DateTime CreatedAt { get; set; } // Timestamp of when ticket was created
    }
}
