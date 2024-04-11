using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Movies.Contracts;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.Movie
{
	/// <summary>
	/// Define mutations for movie create, update.
	/// </summary>
	public class MovieGraphMutation : ObjectGraphType
	{
		public MovieGraphMutation(IMovieGrainClient movieClient)
		{
			Name = "MovieMutations";

			Field<MovieDataGraphType>("create",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<MovieInputGraphType>> {Name = "movie"}),
				resolve: ctx => ResolveGrain(ctx, movieClient)
			);
			Field<MovieDataGraphType>("update",
				arguments: new QueryArguments(
					new QueryArgument<NonNullGraphType<MovieInputGraphType>> {Name = "movie"}),
				resolve: ctx => ResolveGrain(ctx, movieClient)
			);
		}

		private object ResolveGrain(IResolveFieldContext<object> context, IMovieGrainClient movieClient)
		{
			var theMovie = context.GetArgument<Data.Movie>("movie");
			if (theMovie.Id < 1) return movieClient.Create(theMovie);
			return movieClient.Update(theMovie);
		}
	}
}