using System.Threading.Tasks;
using GraphiQl;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Core;
using Movies.GrainClients;
using Movies.Server.Gql.Movie;
using Movies.Server.Infrastructure;

namespace Movies.Server
{
	public class ApiStartup
	{
		private readonly IConfiguration _configuration;
		private readonly IAppInfo _appInfo;

		public ApiStartup(
			IConfiguration configuration,
			IAppInfo appInfo
		)
		{
			_configuration = configuration;
			_appInfo = appInfo;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddCustomAuthentication();
			services.AddCors(o => o.AddPolicy("TempCorsPolicy", builder =>
			{
				builder
					// .SetIsOriginAllowed((host) => true)
					.WithOrigins("http://localhost:6600")
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					;
			}));

			// note: to fix graphql for .net core 3
			services.Configure<KestrelServerOptions>(options =>
			{
				options.AllowSynchronousIO = true;
			});

			services.AddHealthChecks();
			services.AddAppClients();
			services.AddMovieGraphQL();
			services.AddControllers()
				.AddNewtonsoftJson();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env,
			IHostApplicationLifetime life
		)
		{
			app.UseCors("TempCorsPolicy");

			// add http for Schema at default url /graphql
			app.UseGraphQL<ISchema>();

			// use graphql-playground at default url /ui/playground
			app.UseGraphQLPlayground();

			//app.UseGraphQLEndPoint<AppSchema>("/graphql");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseGraphiQl();
			}
			else
			{
				// for remote server to notify developer group of exceptions
				app.UseMiddleware<ExceptionHandlerMiddleware>();
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				// For docker directive to check app status using basic health check
				// HEALTHCHECK CMD curl --fail http://localhost:6600/health || exit
				endpoints.MapHealthChecks("/health");
			});

			//life.ApplicationStarted.Register(OnApplicationStarted(app).Wait);
		}

		/// <summary>
		/// Pre-loads database data at application level.
		/// </summary>
		private Task OnApplicationStarted(IApplicationBuilder app)
		{
			/*var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
			var dataFilename = Path.Combine(path, "movies.json");
			if (!File.Exists(dataFilename)) Console.WriteLine("*** File path not found: {0}", dataFilename);

			Console.WriteLine("Data file name is '{0}'.", dataFilename);

			var grainClient = app.ApplicationServices.GetService<ISampleGrainClient>();
			await grainClient.Configure(dataFilename);*/
			return Task.CompletedTask;
		}
	}
}