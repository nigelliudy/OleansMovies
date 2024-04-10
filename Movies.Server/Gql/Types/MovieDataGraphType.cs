using GraphQL.Types;
using Movies.Contracts;
using Movies.Data;

namespace Movies.Server.Gql.Types
{
	public class MovieDataGraphType : ObjectGraphType<Data.Movie>
	{
		public MovieDataGraphType()
		{
			Name = "Movie";
			Description = "A movie data graphtype.";

			Field(x => x.Id, nullable: true).Description("Unique integer key.");
			Field(x => x.Key, nullable: true).Description("String key.");
			Field(x => x.Name, nullable: true).Description("Name.");
			Field(x => x.Description, nullable: true).Description("Description of movie.");
			Field(x => x.Genres, nullable: true).Description("Genres in string array.");
			Field(x => x.Rate, nullable: true).Description("Movie rating.");
			Field(x => x.Length, nullable: true).Description("Movie duration.");
			Field(x => x.Image, nullable: true).Description("Marketing image.");
		}
	}
}