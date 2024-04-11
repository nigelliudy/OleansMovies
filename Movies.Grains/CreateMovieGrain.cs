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
	public class CreateMovieGrain : Grain, ICreateMovieGrain
	{
		private MoviesContext _moviesContext;

		public CreateMovieGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		public async Task<Movie> CreateMovie(Movie movie)
		{
			movie.Id = await _moviesContext.Movies.MaxAsync(m => m.Id) + 1;
			await _moviesContext.Movies.AddAsync(movie);
			await _moviesContext.SaveChangesAsync();

			return movie;
		}
	}
}