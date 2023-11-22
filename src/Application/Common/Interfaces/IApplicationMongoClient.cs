using JourneyMate.Domain.Entities;
using MongoDB.Driver;

namespace JourneyMate.Application.Common.Interfaces;

public interface IApplicationMongoClient
{
	public IMongoCollection<Report> Reports { get; }
}