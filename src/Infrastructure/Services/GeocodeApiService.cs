using FluentValidation;
using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.ValueObjects;
using JourneyMate.Infrastructure.Common.Models;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

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

	public async Task<AddressAddDto?> GetAddressAsync(string components, string? localityName)
	{
		using var client = new HttpClient();

		var response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/geocode/json?components={components}&key={_keysOptions.Value.GooglePlacesApiKey}&language=pl");
		var addressReadModel = JsonConvert.DeserializeObject<AddressReadModel>(response);
		if (addressReadModel != null)
		{
			if (addressReadModel.Status == "OK")
			{
				var address  = addressReadModel.Results.FirstOrDefault(x => x.Address_Components.Any(y => y.Types.Contains("locality") || y.Types.Contains("administrative_area_level_3")));
				
				if (address != null)
				{
					var isNamevalid  = localityName == address.Address_Components.FirstOrDefault(x => x.Types.Any(y => y.Contains("locality") || y.Contains("administrative_area_level_3")))
						?.Long_Name;
					if (isNamevalid)
					{
						var result = MapToDto(address);
						return result;
					}
					else Log.Error(components + " " + localityName); }
				}
				else Log.Error(components + " " +localityName);
		}

		return null;
	}
	
	public async Task<AddressAddDto?> GetAddressFromTerytAsync(string county, string municipality, bool isCity)
	{
		using var client = new HttpClient();

		var readModel = new TerytReadModel(county,  municipality, isCity);
		var components = readModel.ToComponent();
		var response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/geocode/json?components={components}&key={_keysOptions.Value.GooglePlacesApiKey}&language=pl");
		var addressReadModel = JsonConvert.DeserializeObject<AddressReadModel>(response);
		if (addressReadModel != null)
		{
			if (addressReadModel.Status == "OK")
			{
				var address  = addressReadModel.Results.FirstOrDefault(x => x.Address_Components.Any(y => y.Types.Contains("locality") || y.Types.Contains("administrative_area_level_3") 
					|| y.Types.Contains("administrative_area_level_3"))
					&& !x.Address_Components.Any(y => y.Types.Contains("sublocality")));
				
				if (address != null)
				{
					var name =  address.Address_Components.FirstOrDefault(x => x.Types.Any(y => y.Contains("locality") || y.Contains("administrative_area_level_3")))
						?.Long_Name;
					if (name == municipality)
					{
						var result = MapToDto(address);
						return result;
					}
					else Log.Error(components + " " + municipality);
				}
				else Log.Error(components + " " + municipality);
			}
		}

		readModel.IsCity = !readModel.IsCity;
		components = readModel.ToComponent();
		response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/geocode/json?components={components}&key={_keysOptions.Value.GooglePlacesApiKey}&language=pl");
		addressReadModel = JsonConvert.DeserializeObject<AddressReadModel>(response);
		
		if (addressReadModel != null)
		{
			if (addressReadModel.Status == "OK")
			{
				var address  = addressReadModel.Results.FirstOrDefault(x => x.Address_Components.Any(y => y.Types.Contains("locality") || y.Types.Contains("administrative_area_level_3") 
						|| y.Types.Contains("administrative_area_level_3"))
						&& !x.Address_Components.Any(y => y.Types.Contains("sublocality")));
				
				if (address != null)
				{
					var name =  address.Address_Components.FirstOrDefault(x => x.Types.Any(y => y.Contains("locality") || y.Contains("administrative_area_level_3")))
						?.Long_Name;
					if (name == municipality)
					{
						var result = MapToDto(address);
						return result;
					}
					else Log.Error(components + municipality);
				}
			}
			else Log.Error(components + municipality);
		}

		return null;
	}

	private static AddressAddDto MapToDto(AddressReadModel.Result result)
	{
		var validator = new ResultValidator();
		var validationResult = validator.Validate(result);

		if (!validationResult.IsValid)
		{
			throw new AddressNotFoundException();
		}
		
		var address = new AddressAddDto();
		
		AddressReadModel.AddressComponent locality;
		if (result.Address_Components.Any(x => x.Types.Contains("locality")))
		{
			locality = result.Address_Components.FirstOrDefault(x => x.Types.Contains("locality")) ?? throw new ObjectNotFoundException("Locality");
		}
		else
		{
			locality = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_3")) ?? throw new ObjectNotFoundException("Administrative Area Level_3");
		}
		
		var administrativeAreaLevel2 = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_2")) ?? throw new ObjectNotFoundException("Administrative area level 2");
		var administrativeAreaLevel1 = result.Address_Components.FirstOrDefault(x => x.Types.Contains("administrative_area_level_1")) ?? throw new ObjectNotFoundException("Administrative area level 1");
		var country = result.Address_Components.FirstOrDefault(x => x.Types.Contains("country")) ?? throw new ObjectNotFoundException("Country");
		var postalCode = result.Address_Components.FirstOrDefault(x => x.Types.Contains("postal_code"));

		address.ApiPlaceId = result.Place_Id;
		address.Location = new Location((double)result.Geometry.Location.Lat!, (double)result.Geometry.Location.Lng!);
		address.Locality = new AddressComponent(locality.Short_Name, locality.Long_Name);
		address.AdministrativeArea = new AdministativeAreaAddDto(new AddressComponent(administrativeAreaLevel1.Short_Name, administrativeAreaLevel1.Long_Name), new AddressComponent(administrativeAreaLevel2.Short_Name, administrativeAreaLevel2.Long_Name), new AddressComponent(country.Short_Name, country.Long_Name));
		address.PostalCode = postalCode == null ? "null" : postalCode.Long_Name;


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