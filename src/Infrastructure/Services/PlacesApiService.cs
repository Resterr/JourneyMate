using FluentValidation;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.ValueObjects;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace JourneyMate.Infrastructure.Services;

internal sealed class PlacesApiService : IPlacesApiService
{
	private readonly IOptions<ApiKeysOptions> _keysOptions;
	private readonly IOptions<ApiUrlsOptions> _urlOptions;

	public PlacesApiService(IOptions<ApiUrlsOptions> urlOptions, IOptions<ApiKeysOptions> keysOptions)
	{
		_urlOptions = urlOptions;
		_keysOptions = keysOptions;
	}

	public async Task<List<PlaceDto>> GetPlacesAsync(string location, string radius, string type, string rankBy, double latitude, double longitude)
	{
		using var client = new HttpClient();

		var response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/place/nearbysearch/json" + $"?location={location}&radius={radius}&type={type}&rankby={rankBy}&key={_keysOptions.Value.GooglePlacesApiKey}");

		var placeReadModel = JsonConvert.DeserializeObject<PlaceReadModel>(response);
		var placeReadModels = new List<PlaceReadModel>();
		if (placeReadModel != null)
		{
			placeReadModels.Add(placeReadModel);
			var nextPageToken = placeReadModel.Next_page_token;

			while (!nextPageToken.IsNullOrEmpty())
			{
				Thread.Sleep(2000);
				response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/place/nearbysearch/json" +
					$"?location={location}&radius={radius}&type={type}&rankby={rankBy}&key={_keysOptions.Value.GooglePlacesApiKey}&pagetoken={nextPageToken}");
				placeReadModel = JsonConvert.DeserializeObject<PlaceReadModel>(response);

				if (placeReadModel != null)
				{
					if (placeReadModel.Status != "OK") break;

					nextPageToken = placeReadModel.Next_page_token;
					placeReadModels.Add(placeReadModel);
				}
				else
				{
					break;
				}
			}
		}

		var result = new List<PlaceDto>();
		foreach (var model in placeReadModels)
		{
			var placeDto = MapToDto(model, latitude, longitude);
			result.AddRange(placeDto);
		}

		return result;
	}

	private static List<PlaceDto> MapToDto(PlaceReadModel placeReadModel, double latitude, double longitude)
	{
		var places = new List<PlaceDto>();

		foreach (var result in placeReadModel.Results)
		{
			var validator = new ResultValidator();
			var validationResult = validator.Validate(result);

			if (validationResult.IsValid)
				if (result.Business_status == "OPERATIONAL")
				{
					var place = new PlaceDto
					{
						ApiPlaceId = result.Place_id,
						BusinessStatus = result.Business_status,
						Name = result.Name,
						Rating = result.Rating,
						UserRatingsTotal = result.User_ratings_total,
						Vicinity = result.Vicinity,
						DistanceFromAddress = MathOperations.CalculateDistance(latitude, longitude, (double)result.Geometry.Location.Lat!, (double)result.Geometry.Location.Lng!),
						Location = new Location((double)result.Geometry.Location.Lat!, (double)result.Geometry.Location.Lng!),
						PlusCode = new PlusCode(result.Plus_code.Compound_code, result.Plus_code.Global_code),
						Photo = result.Photos.IsNullOrEmpty() ? null : new Photo(result.Photos[0].Height, result.Photos[0].Height, result.Photos[0].Photo_reference),
						Types = result.Types.Select(x => new PlaceTypeDto
							{
								Name = x
							})
							.ToList()
					};

					places.Add(place);
				}
		}

		return places;
	}

	private class PlaceReadModel
	{
		public List<object> Html_attributions { get; set; }
		public string Next_page_token { get; }
		public List<Result> Results { get; }
		public string Status { get; }

		public class Location
		{
			public double? Lat { get; }
			public double? Lng { get; }
		}

		public class Northeast
		{
			public double Lat { get; set; }
			public double Lng { get; set; }
		}

		public class Southwest
		{
			public double Lat { get; set; }
			public double Lng { get; set; }
		}

		public class Viewport
		{
			public Northeast Northeast { get; set; }
			public Southwest Southwest { get; set; }
		}

		public class Geometry
		{
			public Location Location { get; }
			public Viewport Viewport { get; set; }
		}

		public class OpeningHours
		{
			public bool Open_now { get; set; }
		}

		public class Photo
		{
			public int? Height { get; }
			public List<string> Html_attributions { get; set; }
			public string Photo_reference { get; }
			public int? Width { get; set; }
		}

		public class PlusCode
		{
			public string Compound_code { get; }
			public string Global_code { get; }
		}

		public class Result
		{
			public string Business_status { get; }
			public Geometry Geometry { get; }
			public string Icon { get; set; }
			public string Icon_background_color { get; set; }
			public string Icon_mask_base_uri { get; set; }
			public string Name { get; }
			public OpeningHours Opening_hours { get; set; }
			public List<Photo> Photos { get; }
			public string Place_id { get; }
			public PlusCode Plus_code { get; }
			public double Rating { get; }
			public string Reference { get; set; }
			public string Scope { get; set; }
			public List<string> Types { get; }
			public int User_ratings_total { get; }
			public string Vicinity { get; }
		}
	}

	private class ResultValidator : AbstractValidator<PlaceReadModel.Result>
	{
		public ResultValidator()
		{
			RuleFor(x => x.Business_status)
				.NotNull();
			RuleFor(x => x.Geometry)
				.NotNull();
			RuleFor(x => x.Name)
				.NotNull();
			RuleFor(x => x.Place_id)
				.NotNull();
			RuleFor(x => x.Plus_code)
				.NotNull();
			RuleFor(x => x.Types)
				.NotNull();
			RuleFor(x => x.User_ratings_total)
				.NotNull();
			RuleFor(x => x.Vicinity)
				.NotNull();
		}
	}
}