using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JourneyMate.Domain.Entities;

public class Report
{
	[BsonId]
	[BsonRepresentation(BsonType.String)]
	public Guid Id { get; private set; }
	[BsonRepresentation(BsonType.String)]
	public Guid UserId { get; private set; }
	public int Rating { get; private set; }
	[BsonRepresentation(BsonType.String)]
	public List<Guid> Places { get; private set; }
	
	public Report(Guid id, Guid userId, List<Guid> places)
	{
		Id = id;
		UserId = userId;
		Places = places;
	}

	public void UpdateRating(int rating)
	{
		Rating = rating;
	}
}