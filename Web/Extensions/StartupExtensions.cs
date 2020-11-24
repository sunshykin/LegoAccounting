using LegoAccounting.DAL.MongoDB.Context;
using LegoAccounting.DAL.MongoDB.Infrastructure;
using LegoAccounting.DAL.MongoDB.Repositories;
using LegoAccounting.DAL.Repositories;
using LegoAccounting.Domain.Configuration;
using LegoAccounting.Integration.BrickLink.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace LegoAccounting.Web.Extensions
{
	public static class StartupExtensions
	{
		/// <summary>
		/// Provide configuration elements from IConfiguration
		/// </summary>
		/// <param name="services">Service Collection</param>
		/// <param name="configuration">Configuration</param>
		public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			// Configuring MongoDB settings
			services.Configure<MongoDatabaseConfiguration>(options =>
			{
				options.ConnectionString =
					configuration.GetSection("MongoConnection:ConnectionString").Value;
				options.DatabaseName =
					configuration.GetSection("MongoConnection:Database").Value;
			});

			services.Configure<BrickLinkAuthConfiguration>(options =>
			{
				options.ConsumerKey =
					configuration.GetSection("BrickLinkAuth:ConsumerKey").Value;
				options.ConsumerSecret =
					configuration.GetSection("BrickLinkAuth:ConsumerSecret").Value;
				options.Token =
					configuration.GetSection("BrickLinkAuth:TokenKey").Value;
				options.TokenSecret =
					configuration.GetSection("BrickLinkAuth:TokenSecret").Value;
			});
		}

		/// <summary>
		/// Adds database into DI
		/// </summary>
		/// <param name="services">Service Collection</param>
		public static void AddDatabase(this IServiceCollection services)
		{
			services.AddSingleton<IMongoDatabaseProvider, MongoDatabaseProvider>();
			services.AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<IMongoDatabaseProvider>().GetDatabase());
			services.AddSingleton<MongoContext>();
		}
		
		/// <summary>
		/// Adds repositories into DI
		/// </summary>
		/// <param name="services">Service Collection</param>
		public static void AddRepositories(this IServiceCollection services)
		{
			services.AddSingleton<ISetRepository, SetRepository>();
			services.AddSingleton<IPartRepository, PartRepository>();
			services.AddSingleton<IOrderPriceDataRepository, OrderPriceDataRepository>();
			services.AddSingleton<IStockPriceDataRepository, StockPriceDataRepository>();
			services.AddSingleton<ICollectionItemRepository, CollectionItemRepository>();
			services.AddSingleton<IPartOfSetRepository, PartOfSetRepository>();
			services.AddSingleton<IPartOfCollectionItemRepository, PartOfCollectionItemRepository>();
		}
		
		/// <summary>
		/// Adds integration services into DI
		/// </summary>
		/// <param name="services">Service Collection</param>
		public static void AddIntegrationServices(this IServiceCollection services)
		{
			services.AddSingleton<IBrickLinkDataConnector, BrickLinkDataConnector>();
		}
		
		/// <summary>
		/// Adds services into DI
		/// </summary>
		/// <param name="services">Service Collection</param>
		public static void AddServices(this IServiceCollection services)
		{
		}
	}
}
