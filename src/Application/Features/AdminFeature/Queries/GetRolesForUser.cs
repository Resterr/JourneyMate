using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Models;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AdminFeature.Queries;

public record GetRolesForUser(Guid Id) : IRequest<List<RoleDto>>;

internal sealed class GetRolesForHandler : IRequestHandler<GetRolesForUser, List<RoleDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;
	
	public GetRolesForHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<List<RoleDto>> Handle(GetRolesForUser request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Id == request.Id) ?? throw new UserNotFoundException(request.Id);

		var result = _mapper.Map<List<RoleDto>>(user.Roles);

		return result;
	}
}