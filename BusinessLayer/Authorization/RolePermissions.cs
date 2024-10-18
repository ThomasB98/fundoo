using ModelLayer.Model.Enum;

namespace BusinessLayer.Authorization
{
    public static class RolePermissions
    {
        private static readonly Dictionary<Role, List<Permission>> permissionMap = new()
        {
            {Role.OWNER,new List<Permission>{ Permission.CreateNote, Permission.EditNote, Permission.ViewNote, Permission.DeleteNote, Permission.AddCollaborator, Permission.RemoveCollaborator } },
            {Role.EDITOR,new List<Permission>{ Permission.EditNote, Permission.ViewNote, Permission.AddCollaborator } },
            { Role.VIEWER, new List<Permission> { Permission.ViewNote } }
        };

        public static List<Permission> GetPermissionsForRole(Role role)
        {
            return permissionMap[role];
        }
    }
}
