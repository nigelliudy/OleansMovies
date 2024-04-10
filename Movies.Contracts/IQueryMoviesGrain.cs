using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.Contracts
{
	public interface IQueryMoviesGrain : IGrainWithIntegerKey
	{
		Task<Movie[]> GetHighestRate(int take);
	}
}