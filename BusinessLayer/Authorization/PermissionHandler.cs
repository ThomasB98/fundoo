using BusinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ModelLayer.Model.Enum;
using System.Security.Claims;

namespace BusinessLayer.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly ICollaborationService _collaborationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionHandler(ICollaborationService collaborationService, IHttpContextAccessor httpContextAccessor)
        {
            _collaborationService = collaborationService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Retrieve user ID from claims
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                context.Fail();
                return; // User ID is not found or invalid
            }

            // Retrieve noteId from route (assuming the noteId is passed in route data)
            var noteId = _httpContextAccessor.HttpContext?.Request.RouteValues["noteId"];

            if (noteId == null || !int.TryParse(noteId.ToString(), out int noteIdInt))
            {
                // If noteId is not present, check for userId and return all collaborations
                var userCollaborations = await _collaborationService.GetCollaborationsByUserIdAsync(userId);
                _httpContextAccessor.HttpContext.Items["userCollaborations"] = userCollaborations;
                if (userCollaborations == null)
                {
                    context.Fail();
                    return; // Collaborator not found for this note
                }

                // Get the role from the collaborator entity
                Role userRole = userCollaborations.Data.First().Role;

                // Fetch permissions for the role and check if the required permission is allowed
                var userPermissions = RolePermissions.GetPermissionsForRole(userRole);
                if (userPermissions.Contains(requirement._permission))
                {
                    context.Succeed(requirement); // Permission granted
                }
                else
                {
                    context.Fail(); // Permission denied
                }
                context.Succeed(requirement);
                return;
            }

            // If noteId is present, proceed with the original logic
            var collaborator = await _collaborationService.GetCollaboratorByNoteAndUserAsync(noteIdInt, userId);
            if (collaborator == null)
            {
                context.Fail();
                return; // Collaborator not found for this note
            }

            // Get the role from the collaborator entity
            Role role = collaborator.Role;

            // Fetch permissions for the role and check if the required permission is allowed
            var permissions = RolePermissions.GetPermissionsForRole(role);
            if (permissions.Contains(requirement._permission))
            {
                context.Succeed(requirement); // Permission granted
            }
            else
            {
                context.Fail(); // Permission denied
            }
        }
    }
}