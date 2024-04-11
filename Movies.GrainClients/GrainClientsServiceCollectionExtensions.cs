using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts;

namespace Movies.GrainClients
{
	public static class GrainClientsServiceCollectionExtensions
	{
		/// <summary>
		/// Used to register grain client during startup.
		/// </summary>
		public static void AddAppClients(this IServiceCollection services)
		{
			services.AddSingleton<IMovieGrainClient, MovieGrainClient>();
		}
	}
}