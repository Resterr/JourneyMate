using JourneyMate.Domain.Entities.MongoDb;
using MongoDB.Driver;

namespace JourneyMate.Application.Common.Interfaces;

public interface IApplicationMongoClient
{
	public IMongoCollection<Report> Reports { get; }
}