using Xunit;

namespace Movies.Tests
{
	[CollectionDefinition("default")]
	public class ClusterCollection
	{
		public const string Name = nameof(ClusterCollection);
	}
}