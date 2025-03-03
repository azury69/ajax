using Microsoft.AspNetCore.Identity;
using makingticket.Models;

namespace makingticket.Service
{
    public class RoleManagementService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleManagementService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Check if an admin can modify the user's role
        public async Task<bool> CanModifyUserAsync(string adminId, string targetUserId)
        {
            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null) return false;

            // Prevent modifying the user who assigned them
            return targetUser.AssignedById != adminId;
        }

        // Assign a role to a user
        public async Task<bool> AssignRoleAsync(string adminId, string targetUserId, string role)
        {
            var canModify = await CanModifyUserAsync(adminId, targetUserId);
            if (!canModify) return false;

            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(targetUser);
            await _userManager.RemoveFromRolesAsync(targetUser, currentRoles);
            await _userManager.AddToRoleAsync(targetUser, role);

            targetUser.AssignedById = adminId;  // Track who assigned the role
            await _userManager.UpdateAsync(targetUser);

            return true;
        }
    }
}
