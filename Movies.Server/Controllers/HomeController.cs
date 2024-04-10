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
	public class HomeController : Controller
	{
		private readonly MoviesContext _moviesContext;
		private readonly IMovieGrainClient _client;

		public HomeController(
			MoviesContext moviesContext,
			IMovieGrainClient client
		)
		{
			_moviesContext = moviesContext;
			_client = client;
		}

		// GET api/home
		[HttpGet]
		public async Task<Movie[]> Get()
		{
			var result = await _client
				.GetHighestRate(5)
				.ConfigureAwait(false);
			return result;
		}

		[HttpGet("testcontext")]
		public async Task<Movie[]> GetUsingTestContext()
		{
			return await _moviesContext.Movies
				.OrderByDescending(o => o.Rate)
				.Take(5)
				.ToArrayAsync()
				.ConfigureAwait(false);
		}
	}
}