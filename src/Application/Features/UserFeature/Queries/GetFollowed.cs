using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.UserFeature.Queries;

public record GetFollowed(int PageNumber, int PageSize) : IRequest<PaginatedList<UserNameDto>>;

internal sealed class GetFollowedHandler : IRequestHandler<GetFollowed, PaginatedList<UserNameDto>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;
	private readonly IApplicationDbContext _dbContext;

	public GetFollowedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<UserNameDto>> Handle(GetFollowed request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var followers = await _dbContext.Followers.Include(x => x.Followed)
			.Where(x => x.FollowerId == user.Id)
			.OrderBy(x => x.Followed.UserName)
			.AsNoTracking().PaginatedListAsync(request.PageNumber, request.PageSize);
		var users = followers.Items.Select(x => x.Followed).ToList();
		var userNameDto = _mapper.Map<List<UserNameDto>>(users);
		var result = new PaginatedList<UserNameDto>(userNameDto, followers.TotalCount, request.PageNumber, request.PageSize);

		return result;
	}
}