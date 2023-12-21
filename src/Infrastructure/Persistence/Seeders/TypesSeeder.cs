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
			new("accounting", "Accounting"),
            new("airport", "Airport"),
            new("amusement_park", "Amusement Park"),
            new("aquarium", "Aquarium"),
            new("art_gallery", "Art Gallery"),
            new("atm", "ATM"),
            new("bakery", "Bakery"),
            new("bank", "Bank"),
            new("bar", "Bar"),
            new("beauty_salon", "Beauty Salon"),
            new("bicycle_store", "Bicycle Store"),
            new("book_store", "Book Store"),
            new("bowling_alley", "Bowling Alley"),
            new("bus_station", "Bus Station"),
            new("cafe", "Cafe"),
            new("campground", "Campground"),
            new("car_dealer", "Car Dealer"),
            new("car_rental", "Car Rental"),
            new("car_repair", "Car Repair"),
            new("car_wash", "Car Wash"),
            new("casino", "Casino"),
            new("cemetery", "Cemetery"),
            new("church", "Church"),
            new("city_hall", "City Hall"),
            new("clothing_store", "Clothing Store"),
            new("convenience_store", "Convenience Store"),
            new("courthouse", "Courthouse"),
            new("dentist", "Dentist"),
            new("department_store", "Department Store"),
            new("doctor", "Doctor"),
            new("drugstore", "Drugstore"),
            new("electrician", "Electrician"),
            new("electronics_store", "Electronics Store"),
            new("embassy", "Embassy"),
            new("fire_station", "Fire Station"),
            new("florist", "Florist"),
            new("funeral_home", "Funeral Home"),
            new("furniture_store", "Furniture Store"),
            new("gas_station", "Gas Station"),
            new("gym", "Gym"),
            new("hair_care", "Hair Care"),
            new("hardware_store", "Hardware Store"),
            new("hindu_temple", "Hindu Temple"),
            new("home_goods_store", "Home Goods Store"),
            new("hospital", "Hospital"),
            new("insurance_agency", "Insurance Agency"),
            new("jewelry_store", "Jewelry Store"),
            new("laundry", "Laundry"),
            new("lawyer", "Lawyer"),
            new("library", "Library"),
            new("light_rail_station", "Light Rail Station"),
            new("liquor_store", "Liquor Store"),
            new("local_government_office", "Local Government Office"),
            new("locksmith", "Locksmith"),
            new("lodging", "Lodging"),
            new("meal_delivery", "Meal Delivery"),
            new("meal_takeaway", "Meal Takeaway"),
            new("mosque", "Mosque"),
            new("movie_rental", "Movie Rental"),
            new("movie_theater", "Movie Theater"),
            new("moving_company", "Moving Company"),
            new("museum", "Museum"),
            new("night_club", "Night Club"),
            new("painter", "Painter"),
            new("park", "Park"),
            new("parking", "Parking"),
            new("pet_store", "Pet Store"),
            new("pharmacy", "Pharmacy"),
            new("physiotherapist", "Physiotherapist"),
            new("plumber", "Plumber"),
            new("police", "Police"),
            new("post_office", "Post Office"),
            new("primary_school", "Primary School"),
            new("real_estate_agency", "Real Estate Agency"),
            new("restaurant", "Restaurant"),
            new("roofing_contractor", "Roofing Contractor"),
            new("rv_park", "RV Park"),
            new("school", "School"),
            new("secondary_school", "Secondary School"),
            new("shoe_store", "Shoe Store"),
            new("shopping_mall", "Shopping Mall"),
            new("spa", "Spa"),
            new("stadium", "Stadium"),
            new("storage", "Storage"),
            new("store", "Store"),
            new("subway_station", "Subway Station"),
            new("supermarket", "Supermarket"),
            new("synagogue", "Synagogue"),
            new("taxi_stand", "Taxi Stand"),
            new("tourist_attraction", "Tourist Attraction"),
            new("train_station", "Train Station"),
            new("transit_station", "Transit Station"),
            new("travel_agency", "Travel Agency"),
            new("university", "University"),
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