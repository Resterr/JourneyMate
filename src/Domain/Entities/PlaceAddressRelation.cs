namespace JourneyMate.Domain.Entities;

public class PlaceAddressRelation
{
	public Guid AddressId { get; }
	public Address Address { get; private set; }
	public Guid PlaceId { get; }
	public Place Place { get; private set; }
	public double DistanceFromAddress { get; private set; }

	private PlaceAddressRelation() { }

	public PlaceAddressRelation(Address address, Place place, double distanceFromAddress)
	{
		Address = address;
		Place = place;
		DistanceFromAddress = distanceFromAddress;
	}
}