﻿using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetReportPlacesPaginated(Guid Id, int PageNumber, int PageSize) : IRequest<PaginatedList<PlaceDto>>;

internal sealed class GetReportPlacesPaginatedHandler : IRequestHandler<GetReportPlacesPaginated, PaginatedList<PlaceDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetReportPlacesPaginatedHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlaceDto>> Handle(GetReportPlacesPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var places = await _dbContext.Places.Include(x => x.Reports)
			.Include(x => x.Addresses).Include(x => x.Types)
			.Where(x => x.Reports.Any(y => y.UserId == user.Id) && x.Reports.Any(y => y.Id == request.Id))
			.OrderBy(x => x.Rating)
			.PaginatedListAsync(request.PageNumber, request.PageSize) ?? throw new ReportNotFound(request.Id);
		var placesDto = _mapper.Map<List<PlaceDto>>(places.Items);
		placesDto.ForEach(placeDto => placeDto.DistanceFromAddress = Math.Round(placeDto.DistanceFromAddress, 2));
		var result = new PaginatedList<PlaceDto>(placesDto, places.TotalCount, request.PageNumber, request.PageSize);
		
		return result;
	}
}

public class GetReportPlacesPaginatedValidator : AbstractValidator<GetReportPlacesPaginated>
{
	public GetReportPlacesPaginatedValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty();
		RuleFor(x => x.PageNumber)
			.NotEmpty();
		RuleFor(x => x.PageSize)
			.NotEmpty();
	}
}