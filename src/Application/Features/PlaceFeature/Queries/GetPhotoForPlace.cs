using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace JourneyMate.Application.Features.PlaceFeature.Queries;

public record GetPhotoForPlace(Guid PlaceId, int Height, int Width) : IRequest<Stream>;

internal sealed class GetPhotoForPlaceHandler : IRequestHandler<GetPhotoForPlace, Stream>
{
	private readonly IApplicationDbContext _dbContext;

	public GetPhotoForPlaceHandler(IApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Stream> Handle(GetPhotoForPlace request, CancellationToken cancellationToken)
	{
		var photo = await _dbContext.Photos.SingleOrDefaultAsync(x => x.PlaceId == request.PlaceId);
		if (photo != null)
		{
			var data = photo.Data;
			if (data.Length > 0)
			{
				using (var memoryStream = new MemoryStream(data))
				{
					using (var image = Image.Load(memoryStream))
					{
						image.Mutate(x => x.Resize(new ResizeOptions
						{
							Size = new Size(request.Width, request.Height),
							Mode = ResizeMode.Max
						}));
						
						var resizedMemoryStream = new MemoryStream();
						image.Save(resizedMemoryStream, new JpegEncoder());
						
						resizedMemoryStream.Seek(0, SeekOrigin.Begin);

						return resizedMemoryStream;
					}
				}
			}
		}
		
		throw new PhotoNotFoundException(request.PlaceId);
	}
}

public class GetPhotoForPlaceValidator : AbstractValidator<GetPhotoForPlace>
{
	public GetPhotoForPlaceValidator()
	{
		RuleFor(x => x.PlaceId)
			.NotEmpty();
		RuleFor(x => x.Height)
			.NotEmpty();
		RuleFor(x => x.Width)
			.NotEmpty();
	}
}