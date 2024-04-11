using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	/// <summary>
	/// Interface for movie grain client.
	/// </summary>
	public interface IMovieGrainClient
	{
		Task<Movie[]> GetHighestRate(int take);

		Task<Movie[]> List();

		Task<Movie[]> Search(string searchTerm);

		Task<Movie[]> SearchGenres(string searchTerm);

		Task<Movie> MovieDetails(int id);

		Task<Movie> Create(Movie movie);

		Task<Movie> Update(Movie movie);
	}
}