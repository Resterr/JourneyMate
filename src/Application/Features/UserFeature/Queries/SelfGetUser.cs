using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record SelfGetUser : IRequest<UserDto>;

internal sealed class SelfGetUserHandler : IRequestHandler<SelfGetUser, UserDto>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public SelfGetUserHandler(IApplicationDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
	{
		_dbContext = dbContext;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<UserDto> Handle(SelfGetUser request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.Include(x => x.Roles)
				.AsNoTracking()
				.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken) ??
			throw new UserNotFoundException(userId);
		var result = _mapper.Map<UserDto>(user);

		return result;
	}
}