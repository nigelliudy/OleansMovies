using Xunit;
using Movies.Grains;
using Movies.Tests.Mocks;

namespace Movies.Tests.GrainTests
{
	[Collection("default")]
	public class ListMoviesGrainTest : IClassFixture<MovieDataFixture>
	{
		private readonly MovieDataFixture _fixture;

		public ListMoviesGrainTest(MovieDataFixture fixture)
		{
			_fixture = fixture;
		}
		
		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		public async Task ListMoviesGrain_GetHighestRate_Fetches_Correct_Size_And_In_Order(int take)
		{
			var listGrain = new ListMoviesGrain(_fixture.Context);

			var movies = await listGrain.GetHighestRate(take);
			Assert.Equal(take, movies.Length);
			Assert.EndsWith("Highest", movies[0].Name);
		}
		
		[Fact]
		public async Task ListMoviesGrain_List_Fetches_All()
		{
			var listGrain = new ListMoviesGrain(_fixture.Context);

			var movies = await listGrain.List();
			Assert.Equal(_fixture.Context.Movies.Count(), movies.Length);
		}
		
		[Theory]
		[InlineData("3")]
		[InlineData("Movie")]
		public async Task ListMoviesGrain_Search_Correctly_Returns_Results(string searchTerm)
		{
			var listGrain = new ListMoviesGrain(_fixture.Context);

			var movies = await listGrain.SearchByName(searchTerm);
			Assert.All(movies, movie => Assert.Contains(searchTerm, movie.Name));
		}
		
		[Theory]
		[InlineData("c")]
		[InlineData("d")]
		public async Task ListMoviesGrain_SearchGenres_Correctly_Returns_Results(string genre)
		{
			var listGrain = new ListMoviesGrain(_fixture.Context);

			var movies = await listGrain.SearchGenres(genre);
			Assert.All(movies, movie => Assert.Contains(genre, movie.Genres));
		}
	}
}

