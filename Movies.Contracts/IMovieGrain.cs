using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	/// <summary>
	/// For use by movie grains with state.
	/// </summary>
	public interface IMovieGrain : IGrainWithIntegerKey
	{
		Task<Movie> Get();

		Task<Movie> Set(Movie movie);
	}
}