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

		public Task<Movie[]> GetHighestRate(int take) =>
			_moviesContext.Movies
				.OrderByDescending(o => o.Rate)
				.Take(take)
				.ToArrayAsync();

		public Task<Movie[]> List() =>
			_moviesContext.Movies
				.OrderByDescending(o => o.Id)
				.ToArrayAsync();

		public Task<Movie[]> SearchByName(string name) =>
			_moviesContext.Movies
				.Where(w => EF.Functions.Like(w.Name, $"%{name}%"))
				.ToArrayAsync();

		public Task<Movie[]> SearchGenres(string searchTerm) =>
			_moviesContext.Movies
				.Where(w => StringArrayHas(w.Genres, searchTerm))
				.ToArrayAsync();

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