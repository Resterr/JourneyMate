using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities.MongoDb;
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
		
		var placesDto = report.Places.Select(x => new PlaceDto()
		{
			Id = x
		}).ToList();
		
		var result = placesDto.AsQueryable().PaginatedListSync(request.PageNumber, request.PageSize);
		
		foreach (var placeDto in result.Items)
		{
			var place = await _dbContext.Places.Include(x => x.Addresses)
				.Include(x => x.Types)
				.FirstOrDefaultAsync(x => x.Id == placeDto.Id);

			if (place != null)
			{
				placeDto.UpdateFromPlace(place);
				placeDto.Types = _mapper.Map<List<PlaceTypeDto>>(place.Types);
				placeDto.DistanceFromAddress = place.Addresses.Where(x => x.AddressId == report.AddressId).Select(x => x.DistanceFromAddress).FirstOrDefault();
			}
		}
		
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