using System;
using Movies.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movies.Data;

namespace Movies.Grains
{
	/// <summary>
	/// Creates a new movie data, primary key not used.
	/// </summary>
	[StorageProvider(ProviderName = "Default")]
	public class CreateMovieGrain : Grain, ICreateMovieGrain
	{
		private MoviesContext _moviesContext;

		public CreateMovieGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		/// <summary>
		/// Used by create movie.
		/// </summary>
		public async Task<Movie> CreateMovie(Movie movie)
		{
			// usually in a database, this is handled by an identity/auto-incrementing column
			movie.Id = await _moviesContext.Movies.MaxAsync(m => m.Id) + 1;
			await _moviesContext.Movies.AddAsync(movie);
			try
			{
				await _moviesContext.SaveChangesAsync();
			}
			catch
			{
				var errorMessage = $"CreateMovieGrain: Error writing to database {movie.Name}";
				Console.WriteLine(errorMessage);
				throw new Exception(errorMessage);
			}
			_moviesContext.Entry(movie).State = EntityState.Detached;

			return movie;
		}
	}
}