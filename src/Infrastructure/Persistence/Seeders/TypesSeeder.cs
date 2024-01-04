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
			new PlaceType("amusement_park", "Park rozrywki"),
			new PlaceType("aquarium", "Akwarium"),
			new PlaceType("art_gallery", "Galeria sztuki"),
			new PlaceType("bakery", "Piekarnia"),
			new PlaceType("bar", "Bar"),
			new PlaceType("beauty_salon", "Salon urody"),
			new PlaceType("bowling_alley", "Kręgielnia"),
			new PlaceType("cafe", "Kawiarnia"),
			new PlaceType("campground", "Kemping"),
			new PlaceType("car_rental", "Wypożyczalnia samochodów"),
			new PlaceType("casino", "Kasyno"),
			new PlaceType("cemetery", "Cmentarz"),
			new PlaceType("church", "Kościół"),
			new PlaceType("florist", "Kwiaciarnia"),
			new PlaceType("gas_station", "Stacja paliw"),
			new PlaceType("gym", "Siłownia"),
			new PlaceType("hair_care", "Salon fryzjerski"),
			new PlaceType("hindu_temple", "Świątynia hinduska"),
			new PlaceType("laundry", "Pralnia"),
			new PlaceType("library", "Biblioteka"),
			new PlaceType("light_rail_station", "Stacja kolei linowej"),
			new PlaceType("liquor_store", "Sklep z alkoholem"),
			new PlaceType("lodging", "Zakwaterowanie"),
			new PlaceType("meal_delivery", "Dostawa posiłków"),
			new PlaceType("meal_takeaway", "Posiłki na wynos"),
			new PlaceType("mosque", "Meczet"),
			new PlaceType("movie_rental", "Wypożyczalnia filmów"),
			new PlaceType("movie_theater", "Kino"),
			new PlaceType("museum", "Muzeum"),
			new PlaceType("night_club", "Klub nocny"),
			new PlaceType("park", "Park"),
			new PlaceType("parking", "Parking"),
			new PlaceType("pet_store", "Sklep zoologiczny"),
			new PlaceType("pharmacy", "Apteka"),
			new PlaceType("physiotherapist", "Fizjoterapeuta"),
			new PlaceType("post_office", "Poczta"),
			new PlaceType("restaurant", "Restauracja"),
			new PlaceType("rv_park", "Park dla przyczep kampingowych"),
			new PlaceType("shopping_mall", "Centrum handlowe"),
			new PlaceType("spa", "Spa"),
			new PlaceType("stadium", "Stadion"),
			new PlaceType("store", "Sklep"),
			new PlaceType("subway_station", "Stacja metra"),
			new PlaceType("supermarket", "Supermarket"),
			new PlaceType("synagogue", "Synagoga"),
			new PlaceType("taxi_stand", "Postój taksówek"),
			new PlaceType("tourist_attraction", "Atrakcja turystyczna"),
			new PlaceType("train_station", "Stacja kolejowa"),
			new PlaceType("transit_station", "Stacja przesiadkowa"),
			new PlaceType("travel_agency", "Biuro podróży"),
			new PlaceType("veterinary_care", "Opieka weterynaryjna"),
			new PlaceType("zoo", "Zoo"),
		};

		var currentTypes = _applicationDbContext.PlaceTypes.ToList();
		var typesToAdd = types.Where(item1 => !currentTypes.Any(item2 => item2.ApiName == item1.ApiName))
			.ToList();

		_applicationDbContext.PlaceTypes.AddRange(typesToAdd);
		_applicationDbContext.SaveChanges();
	}
}