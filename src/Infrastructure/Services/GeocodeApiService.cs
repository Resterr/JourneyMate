using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.ValueObjects;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JourneyMate.Infrastructure.Services;

internal sealed class GeocodeApiService : IGeocodeApiService
{
	private readonly IOptions<ApiKeysOptions> _keysOptions;
	private readonly IOptions<ApiUrlsOptions> _urlOptions;

	public GeocodeApiService(IOptions<ApiUrlsOptions> urlOptions, IOptions<ApiKeysOptions> keysOptions)
	{
		_urlOptions = urlOptions;
		_keysOptions = keysOptions;
	}

	public async Task<AddressDto?> GetAddressAsync(string components)
	{
		using var client = new HttpClient();
		var response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/geocode/json?components={components}&key={_keysOptions.Value.GooglePlacesApiKey}");
		var addressReadModel = JsonConvert.DeserializeObject<AddressReadModel>(response);
		if (addressReadModel != null)
		{
			if (addressReadModel.Status == "OK")
			{
				var result = MapToDto(addressReadModel);
				return result;
			}
		}

		return null;
	}

	private static AddressDto MapToDto(AddressReadModel addressReadModel)
	{
		var result = addressReadModel.Results[0];

		var validator = new ResultValidator();
		var validationResult = validator.Validate(result);

		if (!validationResult.IsValid)
		{
			throw new AddressNotFound();
		}

		var address = new AddressDto();

		var locality = result.Address_Components.FirstOrDefault(x => x.Types.Contains("locality")) ?? throw new ObjectNotFound("Locality");
		var administrativeAreaLevel2 = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_2")) ?? throw new ObjectNotFound("Administrative area level 2");
		var administrativeAreaLevel1 = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_1")) ?? throw new ObjectNotFound("Administrative area level 1");
		var country = result.Address_Components.FirstOrDefault(x => x.Types.Contains("country")) ?? throw new ObjectNotFound("Country");
		var postalCode = result.Address_Components.FirstOrDefault(x => x.Types.Contains("postal_code")) ?? throw new ObjectNotFound("Postal code");

		address.PlaceId = result.Place_Id;
		address.Location = new Location((double)result.Geometry.Location.Lat!, (double)result.Geometry.Location.Lng!);
		address.Locality = new AddressComponent(locality.Short_Name, locality.Long_Name);
		address.AdministrativeAreaLevel2 = new AddressComponent(administrativeAreaLevel2.Short_Name, administrativeAreaLevel2.Long_Name);
		address.AdministrativeAreaLevel1 = new AddressComponent(administrativeAreaLevel1.Short_Name, administrativeAreaLevel1.Long_Name);
		address.Country = new AddressComponent(country.Short_Name, country.Long_Name);
		address.PostalCode = new AddressComponent(postalCode.Short_Name, postalCode.Long_Name);


		return address;
	}

	private class AddressReadModel
	{
		public List<Result> Results { get; set; }
		public string Status { get; set; }

		public class AddressComponent
		{
			public string Long_Name { get; set; }
			public string Short_Name { get; set; }
			public List<string> Types { get; set; }
		}

		public class Bounds
		{
			public Location Northeast { get; set; }
			public Location Southwest { get; set; }
		}

		public class Location
		{
			public double? Lat { get; set; }
			public double? Lng { get; set; }
		}

		public class Geometry
		{
			public Bounds Bounds { get; set; }
			public Location Location { get; set; }
			public string Location_Type { get; set; }
			public Bounds Viewport { get; set; }
		}

		public class Result
		{
			public List<AddressComponent> Address_Components { get; set; }
			public string Formatted_Address { get; set; }
			public Geometry Geometry { get; set; }
			public string Place_Id { get; set; }
			public List<string> Types { get; set; }
		}
	}

	private class ResultValidator : AbstractValidator<AddressReadModel.Result>
	{
		public ResultValidator()
		{
			RuleFor(x => x.Address_Components)
				.NotNull();
			RuleFor(x => x.Geometry)
				.NotNull();
		}
	}
}