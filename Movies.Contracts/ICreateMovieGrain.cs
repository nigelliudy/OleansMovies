using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	/// <summary>
	/// Create movie grain interface, separated from update for new ids.
	/// </summary>
	public interface ICreateMovieGrain : IGrainWithGuidKey
	{
		Task<Movie> CreateMovie(Movie movie);
	}
}