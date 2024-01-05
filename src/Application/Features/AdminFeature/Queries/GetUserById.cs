using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.AdminFeature.Queries;

public record GetUserById(Guid Id) : IRequest<UserDto>;

internal sealed class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetUserByIdHandler(IApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<UserDto> Handle(GetUserById request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users.Include(x => x.Roles)
				.AsNoTracking()
				.SingleOrDefaultAsync(x => x.Id == request.Id) ??
			throw new UserNotFoundException(request.Id);

		var result = _mapper.Map<UserDto>(user);

		return result;
	}
}