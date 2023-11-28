using JourneyMate.Application.Common.Interfaces;
using JourneyMate.Domain.Entities.MongoDb;
using MongoDB.Driver;

namespace JourneyMate.Infrastructure.Persistence;
internal class ApplicationMongoClient : MongoClient, IApplicationMongoClient
{
    public IMongoCollection<Report> Reports { get; }
    public ApplicationMongoClient(MongoClientSettings settings) : base(settings)
    {
        var databaseName = "JourneyMate";
        var database = GetDatabase(databaseName);
		Reports = database.GetCollection<Report>("Reports");
    }
}
