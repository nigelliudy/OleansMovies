using System;
using Movies.Contracts;
using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.GrainClients
{
	public class MovieGrainClient : IMovieGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public MovieGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		public Task<Movie[]> GetHighestRate(int take)
		{
			var grain = _grainFactory.GetGrain<IQueryMoviesGrain>(take);
			return grain.GetHighestRate(take);
		}

		public Task<Movie[]> List()
		{
			var grain = _grainFactory.GetGrain<IListMoviesGrain>(0);
			return grain.List();
		}

		public Task<Movie[]> Search(string searchTerm, int take)
		{
			var grain = _grainFactory.GetGrain<IListMoviesGrain>(0);
			return grain.List();
		}

		public Task<int> Create(Movie movie)
		{
			var grain = _grainFactory.GetGrain<ICreateMovieGrain>(Guid.NewGuid());
			return grain.CreateMovie(movie);
		}
	}
}