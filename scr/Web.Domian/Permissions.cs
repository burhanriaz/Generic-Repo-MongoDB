using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api
{
    public static class PermissionRoles
    {
        public const string ReadRole = nameof(ReadRole);
        public const string WriteRole = nameof(WriteRole);
        public const string DeleteRole = nameof(DeleteRole);

    }
    public static class PermissionUsers
    {
        public const string ReadUser = nameof(ReadUser);
        public const string WriteUser = nameof(WriteUser);
        public const string DeleteUser = nameof(DeleteUser);
    }
    
    public static class ClientPermissions
    {
        public const string ReadUser = nameof(ReadUser);

    }
    
  
    public static class AdminPermissions
    {
        // Role Permissions 
        public const string ReadRole = nameof(ReadRole);
        public const string WriteRole = nameof(WriteRole);
        public const string DeleteRole = nameof(DeleteRole);

        // user Permissions 
        public const string ReadUser = nameof(ReadUser);
        public const string WriteUser = nameof(WriteUser);
        public const string DeleteUser = nameof(DeleteUser);

    }

}
