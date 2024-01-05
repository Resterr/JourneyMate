using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record GetFollowers(int PageNumber, int PageSize) : IRequest<PaginatedList<UserNameDto>>;

internal sealed class GetFollowersHandler : IRequestHandler<GetFollowers, PaginatedList<UserNameDto>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetFollowersHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<UserNameDto>> Handle(GetFollowers request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);

		var followers = await _dbContext.Followers.Include(x => x.Follower)
			.Where(x => x.FollowedId == user.Id)
			.OrderBy(x => x.Follower.UserName)
			.AsNoTracking()
			.PaginatedListAsync(request.PageNumber, request.PageSize);
		var users = followers.Items.Select(x => x.Follower)
			.ToList();
		var userNameDto = _mapper.Map<List<UserNameDto>>(users);
		var result = new PaginatedList<UserNameDto>(userNameDto, followers.TotalCount, request.PageNumber, request.PageSize);

		return result;
	}
}