using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Server.Gql.Movie
{
	public class MovieSchema : Schema
	{
		public MovieSchema(IServiceProvider provider)
			: base(provider)
		{
			Query = provider.GetRequiredService<MovieGraphQuery>();
			Mutation = provider.GetRequiredService<MovieGraphMutation>();
		}
	}
}