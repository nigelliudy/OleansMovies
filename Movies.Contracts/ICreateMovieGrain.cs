using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	public interface ICreateMovieGrain : IGrainWithGuidKey
	{
		Task<Movie> CreateMovie(Movie movie);
	}
}