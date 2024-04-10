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
	public class SearchMoviesGrain : Grain, IQueryMoviesGrain
	{
		private MoviesContext _moviesContext;

		public SearchMoviesGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		public Task<Movie[]> GetHighestRate(int take)
		{
			return _moviesContext.Movies
				.OrderByDescending(o => o.Rate)
				.Take(take)
				.ToArrayAsync();
		}
	}
}