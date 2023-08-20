using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Configuration;

namespace JourneyMate.Infrastructure.Persistence.Seeders;
internal interface IUsersSeeder
{
	User SeedSuperAdmin();
	List<Role> SeedDefaultRoles();
}

internal sealed class UsersSeeder : IUsersSeeder
{
    private readonly IConfiguration _configuration;
    private readonly IPasswordManager _passwordManager;

    public UsersSeeder(IConfiguration configuration, IPasswordManager passwordManager)
    {
        _configuration = configuration;
        _passwordManager = passwordManager;
    }


    public User SeedSuperAdmin()
    {
        var superAdminConfig = _configuration.GetOptions<SuperAdminOptions>("SuperAdminAccount");
        var securedPassword = _passwordManager.Secure(superAdminConfig.Password);
        var superAdmin = new User(superAdminConfig.Email, securedPassword, superAdminConfig.UserName);

        return superAdmin;
    }

    public List<Role> SeedDefaultRoles()
	{
        var roles = new List<Role>()
        {
			new Role("SuperAdmin"),
			new Role("Admin"),
			new Role("User"),                  
        };

        return roles;
	}
}
