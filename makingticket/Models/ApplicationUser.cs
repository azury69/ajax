using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace makingticket.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Track the admin who assigned the user's role
        public string? AssignedById { get; set; }

        // Navigation property to access the user who assigned the role
        [ForeignKey("AssignedById")]
        public virtual ApplicationUser? AssignedBy { get; set; }
    }
}
