using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.Movie
{
	public class MovieGraphQuery : ObjectGraphType
	{
		public MovieGraphQuery(IMovieGrainClient movieClient)
		{
			Name = "MovieQueries";

			Field<ListGraphType<MovieDataGraphType>>("home_top_5",
				resolve: ctx => movieClient.GetHighestRate(5)
			);
			Field<ListGraphType<MovieDataGraphType>>("home",
				arguments: new QueryArguments(new QueryArgument<IntGraphType> {Name = "top"}),
				resolve: ctx => movieClient.GetHighestRate(ctx.GetArgument<int>("top"))
			);
		}
	}
}