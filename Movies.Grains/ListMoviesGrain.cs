using System.Linq;
using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Data;

namespace Movies.Grains
{
	[StorageProvider(ProviderName = "Default")]
	public class ListMoviesGrain : Grain, IListMoviesGrain
	{
		private MoviesContext _moviesContext;

		public ListMoviesGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		public Task<Movie[]> List()
		{
			return _moviesContext.Movies
				.OrderByDescending(o => o.Id)
				.ToArrayAsync();
		}
	}
}