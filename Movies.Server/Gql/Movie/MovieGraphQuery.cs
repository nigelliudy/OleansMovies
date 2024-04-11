using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.Movie
{
	public class MovieGraphQuery : ObjectGraphType
	{
		public MovieGraphQuery(IMovieGrainClient movieClient /*, MoviesContext moviesContext*/)
		{
			Name = "MovieQueries";

			Field<ListGraphType<MovieDataGraphType>>("home_top_5",
				resolve: ctx => movieClient.GetHighestRate(5)
			);
			Field<ListGraphType<MovieDataGraphType>>("home",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> {Name = "top"}),
				resolve: ctx => movieClient.GetHighestRate(
					ctx.GetArgument<int>("top", 5)
				)
			);
			Field<ListGraphType<MovieDataGraphType>>("movies_list",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "type"},
					new QueryArgument<StringGraphType> {Name = "search_name"},
					new QueryArgument<StringGraphType> {Name = "search_genres"}),
				resolve: ctx =>
					ctx.GetArgument<string>("type") switch
					{
						"list" => movieClient.List(),
						"search" => movieClient.Search(ctx.GetArgument<string>("search_name", "the")),
						"filter_by_genres" => movieClient.SearchGenres(
							ctx.GetArgument<string>("search_genres", "action")),
						_ => movieClient.List()
					}
			);
			Field<MovieDataGraphType>("movie_detail",
				arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "id"}),
				resolve: ctx =>
				{
					var movieId = ctx.GetArgument<int>("id");
					/*if (!moviesContext.Movies.Any(a => a.Id == movieId))
					{
						throw new ExecutionError($"movie_detail error: Id ({movieId}) does not exist");
					}*/
					return movieClient.MovieDetails(movieId);
				});
		}
	}
}