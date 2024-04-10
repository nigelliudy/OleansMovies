using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	public interface IListMoviesGrain : IGrainWithIntegerKey
	{
		Task<Movie[]> List();
	}
}