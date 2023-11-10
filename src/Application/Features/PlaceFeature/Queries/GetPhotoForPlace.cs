using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Infrastructure.Persistence;
using MediatR;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetPhotoForPlace(Guid PlaceId, int MaxHeight, int MaxWidth) : IRequest<Stream>;

internal sealed class GetPhotoForPlaceHandler : IRequestHandler<GetPhotoForPlace, Stream>
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IPlacesApiService _placesApiService;

	public GetPhotoForPlaceHandler(IApplicationDbContext dbContext, IPlacesApiService placesApiService)
	{
		_dbContext = dbContext;
		_placesApiService = placesApiService;
	}

	public async Task<Stream> Handle(GetPhotoForPlace request, CancellationToken cancellationToken)
	{
		var place = _dbContext.Places.SingleOrDefault(x => x.Id == request.PlaceId) ?? throw new PlaceNotFound(request.PlaceId);
		
		var stream = await _placesApiService.LoadPhoto(place.Photo!.PhotoReference, request.MaxHeight, request.MaxWidth);

		return stream;
	}
}

public class GetPhotoForPlaceValidator : AbstractValidator<GetPhotoForPlace>
{
	public GetPhotoForPlaceValidator()
	{
		RuleFor(x => x.PlaceId)
			.NotNull().NotEmpty();
		
		RuleFor(x => x.MaxHeight)
			.NotNull().GreaterThan(0);
		
		RuleFor(x => x.MaxWidth)
			.NotNull().GreaterThan(0);
	}
}