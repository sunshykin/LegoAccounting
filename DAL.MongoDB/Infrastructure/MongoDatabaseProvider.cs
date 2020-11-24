using LegoAccounting.Domain.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace LegoAccounting.DAL.MongoDB.Infrastructure
{
	public class MongoDatabaseProvider : IMongoDatabaseProvider
	{
		private readonly IMongoDatabase mongoDatabase;

		public MongoDatabaseProvider(IOptions<MongoDatabaseConfiguration> settings)
		{
			// Creating a mongo client
			var client = new MongoClient(settings.Value.ConnectionString);

			// Registering IgnoreExtraElementsConvention for every entity
			ConventionRegistry.Register(
				"Default convention",
				new ConventionPack { new IgnoreExtraElementsConvention(true) },
				_ => true
			);

			// Registering EnumRepresentationConvention for every enum in entity
			ConventionRegistry.Register(
				"EnumStringRepresentation convention",
				new ConventionPack { new EnumRepresentationConvention(BsonType.String) },
				type => true
			);

			mongoDatabase = client.GetDatabase(settings.Value.DatabaseName);
		}

		public IMongoDatabase GetDatabase() => mongoDatabase;
	}
}