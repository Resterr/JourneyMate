namespace JourneyMate.Domain.Entities;

public class PlaceAddress
{
	public Guid AddressId { get; private set; }
	public Address Address { get; private set; }
	public Guid PlaceId { get; private set; }
	public Place Place { get; private set; }
	public double DistanceFromAddress { get; private set; }
	
	private PlaceAddress() { }
	
	public PlaceAddress(Address address, Place place, double distanceFromAddress)
	{
		Address = address;
		Place = place;
		DistanceFromAddress = distanceFromAddress;
	}
}