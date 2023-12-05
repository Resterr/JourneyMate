using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JourneyMate.Domain.Entities.MongoDb;

public class Report
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public Guid Id { get; private set; }
	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; private set; }
	[BsonRepresentation(BsonType.String)]
	public Guid AddressId { get; private set; }
	public int Rating { get; private set; }
	[BsonRepresentation(BsonType.String)]
	public DateTime Created { get; private set; }
	[BsonRepresentation(BsonType.String)]
	public List<Guid> Places { get; private set; }
	public List<string> Types { get; private set; }
	
	private Report() { }
	
	public Report(Guid id, Guid userId, Guid addressId, DateTime created, List<Guid> places, List<string> types)
	{
		Id = id;
		UserId = userId;
		AddressId = addressId;
		Created = created;
		Rating = 0;
		Places = places;
		Types = types;
	}

	public void UpdateRating(int rating)
	{
		Rating = rating;
	}
}