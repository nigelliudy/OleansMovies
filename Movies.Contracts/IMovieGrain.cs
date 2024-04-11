using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	public interface IMovieGrain : IGrainWithIntegerKey
	{
		Task<Movie> Get();

		Task<Movie> Set(Movie movie);
	}
}