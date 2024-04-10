using System;
using System.IO;
using System.Linq;
using Movies.Core;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Data;
using Movies.Server.MovieJsonService;
using Newtonsoft.Json;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace Movies.Server.Infrastructure
{
	public enum StorageProviderType
	{
		Memory
	}

	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public class AppSiloOptions
	{
		private string DebuggerDisplay => $"GatewayPort: '{GatewayPort}', SiloPort: '{SiloPort}'";

		public int GatewayPort { get; set; } = 30000;
		public int SiloPort { get; set; } = 11111;
		public StorageProviderType? StorageProviderType { get; set; }
	}

	public class AppSiloBuilderContext
	{
		public HostBuilderContext HostBuilderContext { get; set; }
		public IAppInfo AppInfo { get; set; }
		public AppSiloOptions SiloOptions { get; set; }
	}

	public static class SiloBuilderExtensions
	{
		private static StorageProviderType _defaultProviderType;

		public static ISiloBuilder UseAppConfiguration(this ISiloBuilder siloHost, AppSiloBuilderContext context)
		{
			_defaultProviderType = context.SiloOptions.StorageProviderType ?? StorageProviderType.Memory;

			var appInfo = context.AppInfo;
			siloHost
				.AddMemoryGrainStorageAsDefault()
				.Configure<ClusterOptions>(options =>
				{
					options.ClusterId = appInfo.ClusterId;
					options.ServiceId = appInfo.Name;
				});

			siloHost.UseDevelopment(context);
			siloHost.UseDevelopmentClustering(context);
			siloHost.UseStartupTasks(context);

			return siloHost;
		}

		private static ISiloBuilder UseDevelopment(this ISiloBuilder siloHost, AppSiloBuilderContext context)
		{
			siloHost
				.ConfigureServices(services =>
				{
					//services.Configure<GrainCollectionOptions>(options => { options.CollectionAge = TimeSpan.FromMinutes(1.5); });
					services.AddDbContextPool<MoviesContext>(options =>
						options.UseInMemoryDatabase("Silo EFCore Movies")
					);
				});

			return siloHost;
		}

		private static ISiloBuilder UseDevelopmentClustering(this ISiloBuilder siloHost, AppSiloBuilderContext context)
		{
			var siloAddress = IPAddress.Loopback;
			var siloPort = context.SiloOptions.SiloPort;
			var gatewayPort = context.SiloOptions.GatewayPort;

			return siloHost
					.UseLocalhostClustering(siloPort: siloPort, gatewayPort: gatewayPort)
				;
		}

		public static ISiloBuilder UseStorage(this ISiloBuilder siloBuilder, string storeProviderName, IAppInfo appInfo,
			StorageProviderType? storageProvider = null, string storeName = null)
		{
			storeName = storeName.IfNullOrEmptyReturn(storeProviderName);
			storageProvider ??= _defaultProviderType;

			switch (storageProvider)
			{
				case StorageProviderType.Memory:
					siloBuilder.AddMemoryGrainStorage(storeProviderName);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(storageProvider),
						$"Storage provider '{storageProvider}' is not supported.");
			}

			return siloBuilder;
		}

		private static ISiloBuilder UseStartupTasks(this ISiloBuilder siloHost, AppSiloBuilderContext context)
		{
			siloHost.AddStartupTask(async (provider, cancellation) =>
			{
				var dbContext = provider.GetService<MoviesContext>();

				await PopulateMoviesFromFile(dbContext, "movies.json", cancellation);
			});

			return siloHost;
		}

		private static async Task PopulateMoviesFromFile(MoviesContext context, string filename,
			CancellationToken cancellation)
		{
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
			var dataFilename = Path.Combine(path, filename);
			if (!File.Exists(dataFilename))
			{
				Console.WriteLine("*** File path not found: {0}", dataFilename);
			}

			await MovieService.Configure(context, dataFilename, cancellation);

			/*await using var fileStream = File.OpenRead(dataFilename);
			using var streamReader = new StreamReader(fileStream);
			using var jsonReader = new JsonTextReader(streamReader);
			// The data is a property "movies" in the json root, 
			// which should be the first array encountered.
			while (jsonReader.TokenType != JsonToken.StartArray)
			{
				await jsonReader.ReadAsync(cancellation);
			}

			var jsonSerializer = new JsonSerializer();
			// Read token by token from the stream
			while (await jsonReader.ReadAsync(cancellation))
			{
				if (jsonReader.TokenType != JsonToken.StartObject)
				{
					continue;
				}

				var theObject = jsonSerializer.Deserialize<Movie>(jsonReader);
				if (theObject is not null && await context.Movies.FindAsync(theObject.Id) == null)
				{
					await context.Movies.AddAsync(theObject, cancellation);
				}
				else
				{
					Console.WriteLine($"Duplicate MovieModel {theObject.Id} : {theObject.Name}");
				}
			}

			await context.SaveChangesAsync(cancellation);

			var firstMovie = context.Movies.First();
			Console.WriteLine($"First movie ({firstMovie.Name}) in the table has Rating of : {firstMovie.Rate}");*/
		}
	}
}