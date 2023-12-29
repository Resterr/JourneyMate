﻿using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Commands;

public record UnfollowUser(string UserName) : IRequest<Unit>;

internal sealed class UnfollowUserHandler : IRequestHandler<UnfollowUser, Unit>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;

	public UnfollowUserHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
	}

	public async Task<Unit> Handle(UnfollowUser request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var userToUnfollow = await _dbContext.Users.Include(x => x.UserFollowers).SingleOrDefaultAsync(x => x.UserName == request.UserName) ?? throw new UserNotFoundException();

		var follow = userToUnfollow.UserFollowers.SingleOrDefault(x => x.FollowerId == user.Id && x.FollowedId == userToUnfollow.Id);
		if(follow == null) throw new AlreadyFollowedException(request.UserName);
		
		userToUnfollow.UserFollowers.Remove(follow);
		await _dbContext.SaveChangesAsync(cancellationToken);
		
		return Unit.Value;
	}
}

public class UnfollowUserValidator : AbstractValidator<UnfollowUser>
{
	public UnfollowUserValidator()
	{
		RuleFor(x => x.UserName)
			.NotEmpty();
	}
}