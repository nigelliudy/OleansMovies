using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	public interface IMovieGrainClient
	{
		Task<Movie[]> GetHighestRate(int take);

		Task<Movie[]> List();

		Task<Movie[]> Search(string searchTerm, int take);

		Task<int> Create(Movie movie);
	}
}