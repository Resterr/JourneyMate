using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Application.Common.Models.Responses;
using JourneyMate.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JourneyMate.Infrastructure.Services;
internal sealed class GeocodeApiService : IGeocodeApiService
{
	private readonly IOptions<ApiUrlsOptions> _urlOptions;
	private readonly IOptions<ApiKeysOptions> _keysOptions;

	public GeocodeApiService(IOptions<ApiUrlsOptions> urlOptions, IOptions<ApiKeysOptions> keysOptions)
	{
		_urlOptions = urlOptions;
		_keysOptions = keysOptions;
	}

	public async Task<AddressResponseDto?> GetAddressAsync(string components)
	{
		using var client = new HttpClient();
		var response = await client.GetStringAsync($"{_urlOptions.Value.GoogleMapsApiUrl}/geocode/json?components={components}&key={_keysOptions.Value.GooglePlacesApiKey}");
		var result = JsonConvert.DeserializeObject<AddressResponseDto>(response);
		return result;
	}
}
