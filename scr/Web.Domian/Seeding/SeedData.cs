using Microsoft.AspNetCore.Identity;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Web.Api;
using Web.Domain;
using Web.Domain.Service.Abstractions;
using Web.Entity.Entity.Identity;
using Web.Entity.UnitOfWork;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

public class SeedData
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IIdentityService identityService;
    private readonly UserManager<Users> userManager;
    private readonly RoleManager<Roles> roleManager;

    public SeedData(IUnitOfWork unitOfWork, IIdentityService _identityService)
    {
        this.unitOfWork = unitOfWork;
        this.identityService = _identityService;

        userManager = identityService.UserManager;
        roleManager = identityService.RoleManager;
    }

    public void Seed()
    {
        var supperAdminRole = CreateSupperAdminRole().GetAwaiter().GetResult();
        if (supperAdminRole != null)
        {
            var superadmin = SeedSupperAdminUser(supperAdminRole.Id).GetAwaiter().GetResult();
        }
        var adminRole = CreateAdminRole().GetAwaiter().GetResult();
        if (adminRole != null)
        {
            var admin = SeedAdminUser(adminRole.Id).GetAwaiter().GetResult();
        }
        var Clientrole = CreateClientRole().GetAwaiter().GetResult();
        // Add more seed methods for other collections if needed
    }

    private async Task<Roles> CreateSupperAdminRole()
    {
        //If there is already an superadmin role, abort
        if (roleManager.Roles.Any(r => r.Name == "superadmin"))
            return null;

        //Create the superadmin Role
        var adminRole = new Roles
        {
            Name = "superadmin",
            NormalizedName = "SUPERADMIN",
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var roleClaim = new Claim(ClaimsConstants.PermissionClaim, ClaimsConstants.SuperAdminPermission);
        await roleManager.CreateAsync(adminRole);
        await roleManager.AddClaimAsync(adminRole, roleClaim);
        return adminRole;
    }
    private async Task<Roles> CreateAdminRole()
    {
        Type t = typeof(AdminPermissions);
        FieldInfo[] fields = t.GetFields();

        var role = await roleManager.FindByNameAsync("admin");
        var roleClaims = new List<Claim>();
        if (role is not null)
        {
            roleClaims = await roleManager.GetClaimsAsync(role) as List<Claim>;
        }
        //Create the client Role if role not exist 
        var clientRole = new Roles();

        if (role is null)
        {
            clientRole = new Roles
            {
                Name = "admin",
                NormalizedName = "ADMIN",
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            //save  role
            await roleManager.CreateAsync(clientRole);
            foreach (var permission in fields)
            {
                var roleClaim = new Claim(ClaimsConstants.PermissionClaim, permission.Name);

                //add claim with role
                await roleManager.AddClaimAsync(clientRole, roleClaim);
            }
            role = clientRole;
            return role;
        }
        else
        {
            foreach (var permission in fields)
            {
                // var claim = new Claim();
                var claim = new Claim(ClaimsConstants.PermissionClaim, permission.Name);

                /// var check = roleClaims.Find(x => x.ClaimValue == permission.Name);
                if (roleClaims.Count == 0)
                {
                    //save claim with role
                    //await _context.RoleClaims.AddAsync(claim);
                    await roleManager.AddClaimAsync(role, claim);

                }
                else if (roleClaims.Find(x => x.Value == permission.Name) == null)
                {
                    await roleManager.AddClaimAsync(role, claim);
                    //save claim with role
                    await roleManager.AddClaimAsync(role, claim);
                }
            }
            return role;
        }
    }
    private async Task<Roles> CreateClientRole()
    {
        Type t = typeof(ClientPermissions);
        FieldInfo[] fields = t.GetFields();

        var role = await roleManager.FindByNameAsync("client");
        var roleClaims = new List<Claim>();
        if (role is not null)
        {
            roleClaims =  await roleManager.GetClaimsAsync(role) as List<Claim>;
        }
        //Create the client Role if role not exist 
        var clientRole = new Roles();

        if (role is null)
        {
            clientRole = new Roles
            {
                Name = "client",
                NormalizedName = "CLIENT",
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };
            //save  role
            await roleManager.CreateAsync(clientRole);
            foreach (var permission in fields)
            {
                var roleClaim = new Claim(ClaimsConstants.PermissionClaim, permission.Name);

                //add claim with role
                await roleManager.AddClaimAsync(clientRole, roleClaim);
            }
            role = clientRole;
            return role;
        }
        else
        {
            foreach (var permission in fields)
            {
               // var claim = new Claim();
                var claim = new Claim(ClaimsConstants.PermissionClaim, permission.Name);

                /// var check = roleClaims.Find(x => x.ClaimValue == permission.Name);
                if (roleClaims.Count == 0)
                {                   
                    //save claim with role
                    //await _context.RoleClaims.AddAsync(claim);
                    await roleManager.AddClaimAsync(role, claim);

                }
                else if (roleClaims.Find(x => x.Value == permission.Name) == null)
                {
                    await roleManager.AddClaimAsync(role, claim);
                    //save claim with role
                    await roleManager.AddClaimAsync(role, claim);
                }
            }
            return role;
        }
    }
    private async Task<Users> SeedSupperAdminUser(Guid roleId)
    {
        // Check if there are existing users
        var userCount = await unitOfWork.GetRepositoryAsync<Users>().Get(x=>x.Roles.Contains(roleId));
        if (userCount.Count() > 0)
        {
            return new Users(); // Database has already been seeded
        }
        var admin = new Users
        {
            FirstName="Supper",
            LastName="Admin",
            UserName = "supperadmin@gmail.com",
            NormalizedUserName = "ADMIN",
            Email = "supperadmin@gmail.com",
            NormalizedEmail = "SUPPERADMIN@GMAIL.COM",
            EmailConfirmed = true,
            //PasswordHash = hasher.HashPassword(null,"Testme@123"),
            SecurityStamp = Guid.NewGuid().ToString(),
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };
        await userManager.CreateAsync(admin, "Testme@123");
        var rolename = await roleManager.FindByIdAsync(roleId.ToString());
        //Adding user to the role
        var roleResult = await userManager.AddToRoleAsync(admin, rolename.Name);
        if (!roleResult.Succeeded)
        {

            return admin;
        }
        return new Users();

    }
    private async Task<Users> SeedAdminUser(Guid roleId)
    {
        // Check if there are existing users
        var userCount = await unitOfWork.GetRepositoryAsync<Users>().Get(x => x.Roles.Contains(roleId));
        if (userCount.Count() > 0)
        {
            return new Users(); // Database has already been seeded
        }
        var admin = new Users
        {
            FirstName = "admin",
            LastName = "",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            //PasswordHash = hasher.HashPassword(null,"Testme@123"),
            SecurityStamp = Guid.NewGuid().ToString(),
            CreateTime = DateTime.UtcNow,
            UpdateTime = DateTime.UtcNow,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };
        await userManager.CreateAsync(admin, "Testme@123");
        var rolename = await roleManager.FindByIdAsync(roleId.ToString());
        //Adding user to the role
        var roleResult = await userManager.AddToRoleAsync(admin, rolename.Name);
        if (!roleResult.Succeeded)
        {

            return admin;
        }
        return new Users();

    }
}
