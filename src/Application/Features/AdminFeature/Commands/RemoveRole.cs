using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AdminFeature.Commands;

public record RemoveRole(Guid Id, string RoleName) : IRequest<Unit>;

internal sealed class RemoveRoleHandler : IRequestHandler<RemoveRole, Unit>
{
	private readonly IApplicationDbContext _dbContext;

	public RemoveRoleHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(RemoveRole request, CancellationToken cancellationToken)
	{
		if (request.RoleName.ToLower() == "superadmin") throw new AccessForbiddenException();

		var user = await _dbContext.Users.Include(x => x.Roles)
				.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ??
			throw new UserNotFoundException(request.Id);

		var roles = user.Roles.Select(x => x.Name)
			.ToList();

		if (roles.Contains("SuperAdmin")) throw new AccessForbiddenException();

		var role = await _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == request.RoleName, cancellationToken);
		if (role != null)
		{
			var isRole = user.Roles.Select(x => x.Name.ToLower())
				.Contains(request.RoleName.ToLower());
			if (!isRole) throw new UserHasNoRoleException(user.Id, request.RoleName);
			user.RemoveRole(role);
		}
		else
		{
			throw new RoleNotFoundException(request.RoleName);
		}

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}

public class RemoveRoleValidator : AbstractValidator<GrantRole>
{
	public RemoveRoleValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
		;
		RuleFor(x => x.RoleName)
			.NotEmpty();
	}
}