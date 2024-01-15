using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Infrastructure.Persistence.Seeders;

internal interface ITypesSeeder
{
	void SeedTypes();
}

internal sealed class TypesSeeder : ITypesSeeder
{
	private readonly IApplicationDbContext _applicationDbContext;

	public TypesSeeder(IApplicationDbContext applicationDbContext)
	{
		_applicationDbContext = applicationDbContext;
	}

	public void SeedTypes()
	{
		var types = new List<PlaceType>
		{
			new("amusement_park", "Park rozrywki"),
			new("aquarium", "Akwarium"),
			new("art_gallery", "Galeria sztuki"),
			new("bakery", "Piekarnia"),
			new("bar", "Bar"),
			new("beauty_salon", "Salon urody"),
			new("bowling_alley", "Kręgielnia"),
			new("cafe", "Kawiarnia"),
			new("campground", "Kemping"),
			new("car_rental", "Wypożyczalnia samochodów"),
			new("casino", "Kasyno"),
			new("cemetery", "Cmentarz"),
			new("church", "Kościół"),
			new("florist", "Kwiaciarnia"),
			new("gas_station", "Stacja paliw"),
			new("gym", "Siłownia"),
			new("hair_care", "Salon fryzjerski"),
			new("hindu_temple", "Świątynia hinduska"),
			new("laundry", "Pralnia"),
			new("library", "Biblioteka"),
			new("light_rail_station", "Stacja kolei linowej"),
			new("liquor_store", "Sklep z alkoholem"),
			new("lodging", "Zakwaterowanie"),
			new("meal_delivery", "Dostawa posiłków"),
			new("meal_takeaway", "Posiłki na wynos"),
			new("mosque", "Meczet"),
			new("movie_rental", "Wypożyczalnia filmów"),
			new("movie_theater", "Kino"),
			new("museum", "Muzeum"),
			new("night_club", "Klub nocny"),
			new("park", "Park"),
			new("parking", "Parking"),
			new("pet_store", "Sklep zoologiczny"),
			new("pharmacy", "Apteka"),
			new("physiotherapist", "Fizjoterapeuta"),
			new("post_office", "Poczta"),
			new("restaurant", "Restauracja"),
			new("rv_park", "Park dla przyczep kampingowych"),
			new("shopping_mall", "Centrum handlowe"),
			new("spa", "Spa"),
			new("stadium", "Stadion"),
			new("store", "Sklep"),
			new("subway_station", "Stacja metra"),
			new("supermarket", "Supermarket"),
			new("synagogue", "Synagoga"),
			new("taxi_stand", "Postój taksówek"),
			new("tourist_attraction", "Atrakcja turystyczna"),
			new("train_station", "Stacja kolejowa"),
			new("transit_station", "Stacja przesiadkowa"),
			new("travel_agency", "Biuro podróży"),
			new("veterinary_care", "Opieka weterynaryjna"),
			new("zoo", "Zoo")
		};

		var currentTypes = _applicationDbContext.PlaceTypes.ToList();
		var typesToAdd = types.Where(item1 => !currentTypes.Any(item2 => item2.ApiName == item1.ApiName))
			.ToList();

		_applicationDbContext.PlaceTypes.AddRange(typesToAdd);
		_applicationDbContext.SaveChanges();
	}
}