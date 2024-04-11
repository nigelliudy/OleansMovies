using GraphQL.Types;

namespace Movies.Server.Gql.Types
{
	/// <summary>
	/// Input argument type for movie mutations.
	/// </summary>
	public class MovieInputGraphType : InputObjectGraphType<Data.Movie>
	{
		public MovieInputGraphType()
		{
			Name = "MovieInput";
			Description = "A movie graphtype for updating and inserting.";

			Field<NonNullGraphType<IntGraphType>>("Id");
			Field<StringGraphType>("Key");
			Field<StringGraphType>("Name");
			Field<StringGraphType>("Description");
			Field<ListGraphType<StringGraphType>>("Genres");
			Field<DecimalGraphType>("Rate");
			Field<StringGraphType>("Length");
			Field<StringGraphType>("Image");
		}
	}
}