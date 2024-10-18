using Microsoft.AspNetCore.Authorization;
using ModelLayer.Model.Enum;

namespace BusinessLayer.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission _permission { get; }

        public PermissionRequirement(Permission permission)
        {
            _permission = permission;
        }
    }
}
