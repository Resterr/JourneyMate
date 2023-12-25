using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record GetFollowers : IRequest<List<string>>;

internal sealed class GetFollowersHandler : IRequestHandler<GetFollowers, List<string>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _dbContext;

	public GetFollowersHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<List<string>> Handle(GetFollowers request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var followers = await _dbContext.Users.Include(x => x.UserFollowers)
			.Where(x => x.UserFollowers.Any(x => x.FollowedId == user.Id))
			.ToListAsync(cancellationToken);
		var result = followers.Select(x => x.UserName).ToList();

		return result;
	}
}