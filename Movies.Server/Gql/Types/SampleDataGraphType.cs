using GraphQL.Types;
using Movies.Contracts;

namespace Movies.Server.Gql.Types
{
	public class SampleDataGraphType : ObjectGraphType<SampleDataModel>
	{
		public SampleDataGraphType()
		{
			Name = "Sample";
			Description = "A sample data graphtype.";

			Field(x => x.Id, true).Description("Unique key.");
			Field(x => x.Name, true).Description("Name.");
		}
	}
}