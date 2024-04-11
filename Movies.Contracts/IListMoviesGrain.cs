using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	/// <summary>
	/// For use by grains that return lists of movie.
	/// </summary>
	public interface IListMoviesGrain : IGrainWithStringKey
	{
		Task<Movie[]> GetHighestRate(int take);

		Task<Movie[]> List();

		Task<Movie[]> SearchByName(string name);

		Task<Movie[]> SearchGenres(string searchTerm);
	}
}