using System;
using Microsoft.AspNetCore.Mvc;
using Movies.Contracts;
using System.Threading.Tasks;
using Movies.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Movies.Server.Controllers
{
	[Route("api/[controller]")]
	public class MovieController : Controller
	{
		private readonly IMovieGrainClient _client;

		public MovieController(
			IMovieGrainClient client
		)
		{
			_client = client;
		}

		// GET api/movie
		[HttpGet]
		public async Task<Movie[]> List()
		{
			var result = await _client
				.List()
				.ConfigureAwait(false);
			return result;
		}

		/*[HttpGet("search/{term}")]
		public async Task<Movie[]> Search()
		{
			return await _moviesContext.Movies
				.OrderByDescending(o => o.Rate)
				.Take(5)
				.ToArrayAsync()
				.ConfigureAwait(false);
		}*/

		[HttpGet("create")]
		public async Task<Movie> Create()
		{
			var result = await _client
				.Create(new Movie
				{
					Key = "newpool",
					Name = "New Pool",
					Description = "test hello world",
					Rate = 5.9m,
					Length = "1hr 18mins",
					Image = "deadpool.jpg"
				})
				.ConfigureAwait(false);
			return result;
		}
	}
}