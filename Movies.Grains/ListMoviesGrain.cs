using System.Globalization;
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
	/// Gathers movie data into a list using various rules.
	/// </summary>
	[StorageProvider(ProviderName = "Default")]
	public class ListMoviesGrain : Grain, IListMoviesGrain
	{
		private MoviesContext _moviesContext;

		public ListMoviesGrain(MoviesContext moviesContext)
		{
			_moviesContext = moviesContext;
		}

		/// <summary>
		/// Used by home. Parameter allows custom number of results. 
		/// </summary>
		public Task<Movie[]> GetHighestRate(int take) =>
			_moviesContext.Movies
				.OrderByDescending(o => o.Rate)
				.Take(take)
				.ToArrayAsync();

		/// <summary>
		/// Used by list movies.
		/// </summary>
		public Task<Movie[]> List() =>
			_moviesContext.Movies
				.OrderByDescending(o => o.Id)
				.ToArrayAsync();

		/// <summary>
		/// Used by search.
		/// </summary>
		public Task<Movie[]> SearchByName(string name) =>
			_moviesContext.Movies
				.Where(w => EF.Functions.Like(w.Name, $"%{name}%"))
				.ToArrayAsync();

		/// <summary>
		/// Used by filter by genres.
		/// </summary>
		public Task<Movie[]> SearchGenres(string searchTerm) =>
			_moviesContext.Movies
				.Where(w => StringArrayHas(w.Genres, searchTerm))
				.ToArrayAsync();

		/// <summary>
		/// For SearchGenres to find if the given term matches any string in a string array. 
		/// </summary>
		private bool StringArrayHas(string[] input, string searchTerm)
		{
			if (input == null || input.Length == 0) return false;

			foreach (var item in input)
				if (item.ToLower(CultureInfo.InvariantCulture)
					.Contains(searchTerm.ToLower(CultureInfo.InvariantCulture)))
					return true;

			return false;
		}
	}
}