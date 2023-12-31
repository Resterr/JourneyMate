using JourneyMate.Application.Common.Exceptions;
using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;
using JourneyMate.Infrastructure.Common.Models;
using JourneyMate.Infrastructure.Common.Options;
using JourneyMate.Infrastructure.TerytWs1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace JourneyMate.Infrastructure.Persistence.Seeders;

internal interface IAdministrativeAreaSeeder
{
	void SeedCountries();
	Task SeedAdministrativeAreas();
}

internal sealed class AdministrativeAreaSeeder : IAdministrativeAreaSeeder
{
	private readonly IApplicationDbContext _dbContext;
	private readonly IGeocodeApiService _geocodeApiService;
	private readonly IDateTimeService _dateTimeService;
	private readonly IOptions<ApiKeysOptions> _keysOptions;

	public AdministrativeAreaSeeder(IApplicationDbContext dbContext, IGeocodeApiService geocodeApiService, IDateTimeService dateTimeService, IOptions<ApiKeysOptions> keysOptions)
	{
		_dbContext = dbContext;
		_geocodeApiService = geocodeApiService;
		_dateTimeService = dateTimeService;
		_keysOptions = keysOptions;
	}

	public void SeedCountries()
	{
		var newCountry = new Country("PL", "Poland");
		
		if (!_dbContext.Countries.Any(x => x.LongName == newCountry.LongName))
		{
			_dbContext.Countries.Add(newCountry);
			_dbContext.SaveChanges();
		}
	}

	public async Task SeedAdministrativeAreas()
	{
		var addressCount = await _dbContext.Addresses.CountAsync();
		if(addressCount == 0)
		{
			var country = await _dbContext.Countries.SingleOrDefaultAsync(x => x.LongName == "Poland") ?? throw new ObjectNotFoundException("Country");
			var terytReadModel = await GetAdministrativeAreas();
			foreach (var teryt in terytReadModel)
			{
				var component = teryt.ToComponent();
				var address = await _geocodeApiService.GetAddressFromTerytAsync(teryt.County, teryt.Municipality, teryt.IsCity);
				if (address != null)
				{
					var level1 = _dbContext.AdministrativeAreaLevel1.SingleOrDefault(x => x.LongName == address.AdministrativeArea.Level1.LongName);
					if (level1 == null)
					{
						level1 = new AdministrativeAreaLevel1(address.AdministrativeArea.Level1.ShortName, address.AdministrativeArea.Level1.LongName, country);
						_dbContext.AdministrativeAreaLevel1.Add(level1);
						_dbContext.SaveChanges();
					}

					
					var level2 = _dbContext.AdministrativeAreaLevel2.SingleOrDefault(x => x.LongName == address.AdministrativeArea.Level2.LongName);
					if (level2 == null)
					{
						level2 = new AdministrativeAreaLevel2(address.AdministrativeArea.Level2.ShortName, address.AdministrativeArea.Level2.LongName, level1);
						_dbContext.AdministrativeAreaLevel2.Add(level2);
						_dbContext.SaveChanges();
					}

					var locality = _dbContext.Addresses.SingleOrDefault(x => x.Locality.LongName == address.Locality.LongName);
					if (locality == null)
					{
						locality = new Address(address.ApiPlaceId, address.Locality, address.Location, level2, address.PostalCode);
						_dbContext.Addresses.Add(locality);
					}
				}
			}
			
			_dbContext.SaveChanges();
		}
	}
	
	private async Task<List<TerytReadModel>> GetAdministrativeAreas()
	{
		var result = new List<TerytReadModel>();
		using var client = new TerytWs1Client();
		client.ClientCredentials.UserName.UserName = _keysOptions.Value.TerytUserName;
		client.ClientCredentials.UserName.Password = _keysOptions.Value.TerytPassword;
		
		if(await client.CzyZalogowanyAsync())
		{
			var voivodeships = await client.PobierzListeWojewodztwAsync(_dateTimeService.CurrentDate());
			foreach (var voivodeship in voivodeships)
			{
				var counties = await client.PobierzListePowiatowAsync(voivodeship.WOJ, _dateTimeService.CurrentDate());
				
				foreach (var county in counties)
				{
					var municipalities = await client.PobierzListeGminAsync(county.WOJ, county.POW, _dateTimeService.CurrentDate());
					var municipalitiesFiltered = municipalities.DistinctBy(x => x.NAZWA).ToArray();
					foreach (var municipality in municipalitiesFiltered)
					{
						if (municipality.RODZ == "8" || municipality.RODZ == "9") continue;
						
						var area = county.NAZWA_DOD.ToLower().Contains("miasto") ? county.NAZWA : county.NAZWA_DOD + " " + county.NAZWA;
						var isCity  = municipalities.Where(x => x.NAZWA == municipality.NAZWA)
							.Any(x => x.RODZ.Contains("1") || x.RODZ.Contains("3") || x.RODZ.Contains("4") || x.RODZ.Contains("5"));
						
						var readmodel = new TerytReadModel(area, municipality.NAZWA, isCity);
						result.Add(readmodel);
					}
				}
			}
		}

		return result;
	}
}