using GraphQL.Types;

namespace Movies.Server.Gql.Types
{
	/// <summary>
	/// Output type for movie queries, and mutations.
	/// </summary>
	public class MovieDataGraphType : ObjectGraphType<Data.Movie>
	{
		public MovieDataGraphType()
		{
			Name = "Movie";
			Description = "A movie data graphtype.";

			Field(x => x.Id, true).Description("Unique integer key.");
			Field(x => x.Key, true).Description("String key.");
			Field(x => x.Name, true).Description("Name.");
			Field(x => x.Description, true).Description("Description of movie.");
			Field(x => x.Genres, true).Description("Genres in string array.");
			Field(x => x.Rate, true).Description("Movie rating.");
			Field(x => x.Length, true).Description("Movie duration.");
			Field(x => x.Image, true).Description("Marketing image.");
		}
	}
}