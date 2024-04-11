using GraphQL.Server;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Movies.Server.Gql.Types;

namespace Movies.Server.Gql.Movie
{
	public static class MovieGqlExtensions
	{
		public static void AddMovieGraphQL(this IServiceCollection services)
		{
			services.AddGraphQL(options =>
				{
					options.EnableMetrics = true;
					options.ExposeExceptions = true;
				})
				.AddNewtonsoftJson();

			services.AddSingleton<ISchema, MovieSchema>();
			services.AddSingleton<MovieGraphQuery>();
			services.AddSingleton<MovieGraphMutation>();

			services.AddSingleton<MovieDataGraphType>();
			services.AddSingleton<MovieInputGraphType>();
		}
	}
}