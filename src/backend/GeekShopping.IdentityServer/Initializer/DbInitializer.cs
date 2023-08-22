using IdentityModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model.Context;

namespace GeekShopping.IdentityServer.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly MySqlContext _context;
    private readonly UserManager<ApplicationUser> _user;
    private readonly RoleManager<IdentityRole> _role;

    public DbInitializer(MySqlContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
    {
        _context = context;
        _user = user;
        _role = role;
    }

    public void Initialize()
    {
        if (_role.FindByIdAsync(IdentityConfiguration.Admin).Result != null)
        {
            return;
        }
        _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin))
            .GetAwaiter().GetResult();
        _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client))
            .GetAwaiter().GetResult();

        ApplicationUser admin = new ApplicationUser
        {
            UserName = "alisson-admin",
            Email = "alisson-admin@teste.com.br",
            EmailConfirmed = true,
            PhoneNumber = "+55 (11) 98745-6321",
            FirtName = "Alisson",
            LastName = "Admin"
        };

        _user.CreateAsync(admin, "Admin123@").GetAwaiter().GetResult();
        _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

        var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, $"{admin.FirtName} {admin.LastName}"),
            new Claim(JwtClaimTypes.GivenName, admin.FirtName),
            new Claim(JwtClaimTypes.FamilyName, admin.LastName),
            new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
        }).Result;

        ApplicationUser client = new ApplicationUser
        {
            UserName = "alisson-client",
            Email = "alisson-client@teste.com.br",
            EmailConfirmed = true,
            PhoneNumber = "+55 (11) 98745-6321",
            FirtName = "Alisson",
            LastName = "Client"
        };

        _user.CreateAsync(client, "Admin123@").GetAwaiter().GetResult();
        _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

        var clientClaims = _user.AddClaimsAsync(client, new Claim[]
        {
            new Claim(JwtClaimTypes.Name, $"{client.FirtName} {admin.LastName}"),
            new Claim(JwtClaimTypes.GivenName, client.FirtName),
            new Claim(JwtClaimTypes.FamilyName, client.LastName),
            new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin),
        }).Result;
    }
}
