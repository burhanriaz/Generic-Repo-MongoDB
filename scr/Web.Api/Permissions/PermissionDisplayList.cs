namespace Web.Api
{
    public static class PermissionDisplayList
    {
        public static List<string> Roles = new()
        {
            PermissionRoles.ReadRole,
            PermissionRoles.WriteRole,
            PermissionRoles.DeleteRole,
        };
        public static List<string> Users = new()
        {
            PermissionUsers.ReadUser,
            PermissionUsers.WriteUser,
            PermissionUsers.DeleteUser,
        };


        public static IEnumerable<ModulesPermission> ModulesPermissions = new List<ModulesPermission>()
        {
            new("Roles", Roles),
            new("Users", Users),
        };
    }

    public class ModulesPermission
    {
        public string Title { get; set; }
        public string Key { get; set; }
        public List<string> Children { get; set; }

        public ModulesPermission(string moduleName, List<string> permissionModels)
        {
            Title = moduleName;
            Key = Title;
            Children = permissionModels;
        }
    }
}
