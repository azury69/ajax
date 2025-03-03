namespace makingticket.Models
{
    internal class RoleManagementViewModel
    {
        public List<(ApplicationUser user, IList<string> roles)>? UsersWithRoles { get; set; }
        public ApplicationUser? CurrentUser { get; set; }
    }
}