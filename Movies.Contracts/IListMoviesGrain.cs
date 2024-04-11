using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	public interface IListMoviesGrain : IGrainWithStringKey
	{
		Task<Movie[]> GetHighestRate(int take);

		Task<Movie[]> List();

		Task<Movie[]> SearchByName(string name);

		Task<Movie[]> SearchGenres(string searchTerm);
	}
}