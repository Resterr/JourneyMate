using AutoMapper;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetAllReports : IRequest<List<ReportDto>>;

internal sealed class GetAllReportsHandler : IRequestHandler<GetAllReports, List<ReportDto>>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IApplicationMongoClient _mongoClient;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetAllReportsHandler(IApplicationDbContext dbContext, IApplicationMongoClient mongoClient, ICurrentUserService currentUserService, IMapper mapper)
	{
		_dbContext = dbContext;
		_mongoClient = mongoClient;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<List<ReportDto>> Handle(GetAllReports request, CancellationToken cancellationToken)
	{
		var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
		var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UserNotFoundException(userId);
		var filter = Builders<Report>.Filter.Eq(x => x.UserId, user.Id);
		var reports = await _mongoClient.Reports.Find(filter)
			.ToListAsync();

		var result = new List<ReportDto>();
		foreach (var report in reports)
		{
			var placesDto = new List<PlaceDto>();
			if (report.Places.Count > 0)
			{
				foreach (var placeDtoId in report.Places)
				{
					var place = await _dbContext.Places.Include(x => x.Address)
						.FirstOrDefaultAsync(x => x.Id == placeDtoId);

					if (place != null)
					{
						placesDto.Add(_mapper.Map<PlaceDto>(place));
					}
				}
			}
			
			var reportDto = new ReportDto()
			{
				Id = report.Id,
				Rating = report.Rating,
				Places = placesDto
			};
			
			result.Add(reportDto);
		}

		return result;
	}
}