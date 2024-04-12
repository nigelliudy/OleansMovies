using System;
using Microsoft.EntityFrameworkCore;
using Movies.Data;

namespace Movies.Tests.Mocks
{
	public class MovieDataFixture : IDisposable
	{
		private static readonly DbContextOptionsBuilder<MoviesContext> OptionsBuilder = new DbContextOptionsBuilder<MoviesContext>()
			.UseInMemoryDatabase("Movie Data Test");
		public MoviesContext Context { get; private set; } = new MoviesContext(OptionsBuilder.Options);

		public MovieDataFixture()
		{
			Context.Movies.Add(new Movie {Id = 1, Name = "Movie 1", Genres = new [] {"a", "b", "c"}, Rate = 9.0m});
			Context.Movies.Add(new Movie {Id = 2, Name = "Movie 2 : Highest", Genres = new [] {"d"}, Rate = 9.5m});
			Context.Movies.Add(new Movie {Id = 3, Name = "Movie 3", Genres = new [] {"e", "c"}, Rate = 8.0m});
			Context.Movies.Add(new Movie {Id = 4, Name = "Movie 4", Genres = new [] {"f"}, Rate = 8.5m});
			Context.Movies.Add(new Movie {Id = 5, Name = "Movie 5", Genres = new [] {"g"}, Rate = 7.0m});
			Context.SaveChanges();
		}

		void IDisposable.Dispose() => Context.Dispose();
	}
}