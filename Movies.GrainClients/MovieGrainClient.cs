using System;
using Movies.Contracts;
using Orleans;
using System.Threading.Tasks;
using Movies.Data;

namespace Movies.GrainClients
{
	/// <summary>
	/// Used by Graph query or mutation to execute logic that is in grains.
	/// </summary>
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
			var grain = _grainFactory.GetGrain<IListMoviesGrain>($"top {take}");
			return grain.GetHighestRate(take);
		}

		public Task<Movie[]> List()
		{
			var grain = _grainFactory.GetGrain<IListMoviesGrain>($"list {DateTime.Now}");
			return grain.List();
		}

		public Task<Movie[]> Search(string searchTerm)
		{
			var grain = _grainFactory.GetGrain<IListMoviesGrain>($"search by name {searchTerm}");
			return grain.SearchByName(searchTerm);
		}

		public Task<Movie[]> SearchGenres(string searchTerm)
		{
			var grain = _grainFactory.GetGrain<IListMoviesGrain>($"search by genre {searchTerm}");
			return grain.SearchGenres(searchTerm);
		}

		public Task<Movie> MovieDetails(int id)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(id);
			return grain.Get();
		}

		public Task<Movie> Create(Movie movie)
		{
			var grain = _grainFactory.GetGrain<ICreateMovieGrain>(Guid.NewGuid());
			return grain.CreateMovie(movie);
		}

		public Task<Movie> Update(Movie movie)
		{
			var grain = _grainFactory.GetGrain<IMovieGrain>(movie.Id);
			return grain.Set(movie);
		}
	}
}