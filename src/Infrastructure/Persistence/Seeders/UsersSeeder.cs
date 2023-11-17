using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JourneyMate.Infrastructure.Persistence.Seeders;

internal interface IUsersSeeder
{
	void SeedDefaultRoles();
	void SeedSuperAdmin();
}

internal sealed class UsersSeeder : IUsersSeeder
{
	private readonly IConfiguration _configuration;
	private readonly IApplicationDbContext _applicationDbContext;
	private readonly IPasswordManager _passwordManager;

	public UsersSeeder(IConfiguration configuration, IApplicationDbContext applicationDbContext, IPasswordManager passwordManager)
	{
		_configuration = configuration;
		_applicationDbContext = applicationDbContext;
		_passwordManager = passwordManager;
	}

	public void SeedDefaultRoles()
	{
		var roles = new List<Role>
		{
			new("SuperAdmin"),
			new("Admin"),
			new("User")
		};

		var currentRoles = _applicationDbContext.Roles.ToList();
		var categoriesToAdd = roles.Where(item1 => !currentRoles.Any(item2 => item2.Name == item1.Name))
			.ToList();

		_applicationDbContext.Roles.AddRange(categoriesToAdd);
		_applicationDbContext.SaveChanges();
	}

	public void SeedSuperAdmin()
	{
		var superAdminConfig = _configuration.GetOptions<SuperAdminOptions>("SuperAdminAccount");
		var securedPassword = _passwordManager.Secure(superAdminConfig.Password);
		var superAdmin = new User(superAdminConfig.Email, securedPassword, superAdminConfig.UserName);
		var superAdminRole = _applicationDbContext.Roles.Single(x => x.Name == "SuperAdmin");
		var adminRole = _applicationDbContext.Roles.Single(x => x.Name == "Admin");
		var userRole = _applicationDbContext.Roles.Single(x => x.Name == "User");

		var isSuperAdminExists = _applicationDbContext.Users.Include(x => x.Roles)
			.Any(x => x.Roles.Contains(superAdminRole));

		if (!isSuperAdminExists)
		{
			superAdmin.Roles.Add(superAdminRole);
			superAdmin.Roles.Add(adminRole);
			superAdmin.Roles.Add(userRole);
			_applicationDbContext.Users.Add(superAdmin);
			_applicationDbContext.SaveChanges();
		}
	}
}