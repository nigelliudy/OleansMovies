using System;
using System.Linq;
using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Data;

namespace Movies.Grains
{
	/// <summary>
	/// Holds a single movie data, with caching on Get(). Set() always writes through the caching.
	/// </summary>
	[StorageProvider(ProviderName = "Default")]
	public class MovieGrain : Grain<Movie>, IMovieGrain
	{
		private static TimeSpan _cacheLength = new TimeSpan(0, 0, 20);
		private readonly MoviesContext _moviesContext;
		private DateTime _lastUpdate;

		public MovieGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		/// <summary>
		/// Used by movie detail. Uses the State as cache, with 'CACHE_LENGTH' duration. 
		/// </summary>
		public Task<Movie> Get()
		{
			if (_lastUpdate + _cacheLength < DateTime.Now)
			{
				var movie = _moviesContext.Movies
					.AsNoTracking()
					.SingleOrDefault(s => s.Id == this.GetPrimaryKeyLong());
				State = movie;
				_lastUpdate = DateTime.Now;
			}
			/*else
				Console.WriteLine($"MovieGrain: Read from cache {State?.Name}");*/

			return Task.FromResult(State);
		}

		/// <summary>
		/// Used by update movie.
		/// </summary>
		public async Task<Movie> Set(Movie movie)
		{
			_moviesContext.Movies.Update(movie);
			try
			{
				await _moviesContext.SaveChangesAsync();
			}
			catch
			{
				var errorMessage = $"MovieGrain: Error writing to database {movie.Name}";
				Console.WriteLine(errorMessage);
				throw new Exception(errorMessage);
			}
			_moviesContext.Entry(movie).State = EntityState.Detached;
			_lastUpdate = DateTime.Now;
			State = movie;

			return State;
		}
	}
}