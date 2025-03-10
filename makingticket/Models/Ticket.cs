using System;
using System.Collections.Generic;

namespace makingticket.Models
{
    // Enum for ticket priority
    public enum TicketPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    // Enum for ticket status
    public enum TicketStatus
    {
        New,
        InProgress,
        Resolved,
        Done
    }

    public class Ticket
    {
        public int Id { get; set; }

        // Ticket title or summary
        public string Name { get; set; }

        // Detailed description of the ticket
        public string Description { get; set; }

        // The user assigned to the ticket (use foreign key for real implementation)
        public string AssignedTo { get; set; }  // Consider using UserId for foreign key in real implementation

        // Priority of the ticket
        public TicketPriority Priority { get; set; }

        // Story points estimate (effort required)
        public int StoryPoints { get; set; }

        // Current status of the ticket
        public TicketStatus Status { get; set; }

        // Due date for the ticket (can be null if not set)
        public DateTime? DueDate { get; set; }

        // The date when the ticket was created
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // A list of labels or tags for categorization (e.g., "Bug", "Feature", "Urgent")
        public string Labels { get; set; }

        // Epic link, in case you need to associate the ticket with a larger feature/initiative
        public string Epic { get; set; }

        // Collection of file paths/URLs for attachments
        //public List<string> Attachments { get; set; } = new List<string>();

        // Collection of comments related to the ticket (can be multiple comments from users)
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

    // Comment model to store individual comments
    public class Comment
    {
        public int Id { get; set; }

        // The user who made the comment
        public string UserName { get; set; }

        // The actual content of the comment
        public string Content { get; set; }

        // When the comment was created
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
