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
		private static TimeSpan CACHE_LENGTH = new TimeSpan(0, 0, 20);
		private readonly MoviesContext _moviesContext;
		private DateTime _lastUpdate;

		public MovieGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		public Task<Movie> Get()
		{
			if (_lastUpdate + CACHE_LENGTH < DateTime.Now)
			{
				var movie = _moviesContext.Movies
					.AsNoTracking()
					.SingleOrDefault(s => s.Id == this.GetPrimaryKeyLong());
				State = movie;
				_lastUpdate = DateTime.Now;
				Console.WriteLine($"MovieGrain: Read from database {movie?.Name}");
			}
			else
				Console.WriteLine($"MovieGrain: Read from cache {State?.Name}");

			return Task.FromResult(State);
		}

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

			Console.WriteLine($"MovieGrain: Write to database {movie.Name}");
			return State;
		}
	}
}