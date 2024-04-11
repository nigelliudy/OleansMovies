using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.Server.Gql.Movie
{
	/// <summary>
	/// Define schema for movie queries and mutations.
	/// </summary>
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