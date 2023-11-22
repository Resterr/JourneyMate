using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetReportPlacesPaginated(Guid Id, int PageNumber, int PageSize) : IRequest<PaginatedList<PlaceDto>>;

internal sealed class GetReportPlacesPaginatedHandler : IRequestHandler<GetReportPlacesPaginated, PaginatedList<PlaceDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IApplicationMongoClient _mongoClient;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetReportPlacesPaginatedHandler(IApplicationDbContext dbContext, IApplicationMongoClient mongoClient, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_mongoClient = mongoClient;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<PaginatedList<PlaceDto>> Handle(GetReportPlacesPaginated request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var filter = Builders<Report>.Filter.Eq(x => x.Id, request.Id) & Builders<Report>.Filter.Eq(x => x.UserId, user.Id);
		var report = await _mongoClient.Reports.Find(filter)
				.FirstOrDefaultAsync() ??
			throw new ReportNotFound(request.Id);

		var reportPlacePaginated = new PaginatedList<Guid>(report.Places, report.Places.Count(), request.PageNumber, request.PageSize);
		var placesDto = new List<PlaceDto>();

		foreach (var placeDtoId in reportPlacePaginated.Items)
		{
			var place = await _dbContext.Places.Include(x => x.Addresses)
				.Include(x => x.Types)
				.FirstOrDefaultAsync(x => x.Id == placeDtoId);

			if (place != null)
			{
				placesDto.Add(_mapper.Map<PlaceDto>(place));
			}
		}
		
		var result = new PaginatedList<PlaceDto>(placesDto, report.Places.Count(), request.PageNumber, request.PageSize);

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