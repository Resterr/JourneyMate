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
            new("amusement_park", "Amusement Park"),
            new("aquarium", "Aquarium"),
            new("art_gallery", "Art Gallery"),
            new("bakery", "Bakery"),
            new("bar", "Bar"),
            new("beauty_salon", "Beauty Salon"),
            new("bowling_alley", "Bowling Alley"),
            new("cafe", "Cafe"),
            new("campground", "Campground"),
            new("car_rental", "Car Rental"),
            new("casino", "Casino"),
            new("cemetery", "Cemetery"),
            new("church", "Church"),
            new("florist", "Florist"),
            new("gas_station", "Gas Station"),
            new("gym", "Gym"),
            new("hair_care", "Hair Care"),
            new("hindu_temple", "Hindu Temple"),
            new("laundry", "Laundry"),
            new("library", "Library"),
            new("light_rail_station", "Light Rail Station"),
            new("liquor_store", "Liquor Store"),
            new("lodging", "Lodging"),
            new("meal_delivery", "Meal Delivery"),
            new("meal_takeaway", "Meal Takeaway"),
            new("mosque", "Mosque"),
            new("movie_rental", "Movie Rental"),
            new("movie_theater", "Movie Theater"),
            new("museum", "Museum"),
            new("night_club", "Night Club"),
            new("park", "Park"),
            new("parking", "Parking"),
            new("pet_store", "Pet Store"),
            new("pharmacy", "Pharmacy"),
            new("physiotherapist", "Physiotherapist"),
            new("post_office", "Post Office"),
            new("restaurant", "Restaurant"),
            new("rv_park", "RV Park"),
            new("shopping_mall", "Shopping Mall"),
            new("spa", "Spa"),
            new("stadium", "Stadium"), 
			new("store", "Store"),
            new("subway_station", "Subway Station"),
            new("supermarket", "Supermarket"),
            new("synagogue", "Synagogue"),
            new("taxi_stand", "Taxi Stand"),
            new("tourist_attraction", "Tourist Attraction"),
            new("train_station", "Train Station"),
            new("transit_station", "Transit Station"),
            new("travel_agency", "Travel Agency"),
            new("veterinary_care", "Veterinary Care"),
            new("zoo", "Zoo"),
		};

		var currentTypes = _applicationDbContext.PlaceTypes.ToList();
		var typesToAdd = types.Where(item1 => !currentTypes.Any(item2 => item2.ApiName == item1.ApiName))
			.ToList();

		_applicationDbContext.PlaceTypes.AddRange(typesToAdd);
		_applicationDbContext.SaveChanges();
	}
}