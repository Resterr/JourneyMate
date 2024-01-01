using AutoMapper;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AdminFeature.Queries;

public record GetAllUsers : IRequest<List<UserDto>>;

internal sealed class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllUsersHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<List<UserDto>> Handle(GetAllUsers request, CancellationToken cancellationToken)
	{
		var users = await _dbContext.Users.Include(x => x.Roles)
			.AsNoTracking()
			.ToListAsync();
		var superAdmin = users.Where(user => user.Roles.Any(role => role.Name == "SuperAdmin"))
			.ToList();

		users.Remove(superAdmin[0]);
		
		var result = _mapper.Map<List<UserDto>>(users);

		return result;
	}
}