using System.Threading.Tasks;
using Orleans.TestingHost;
using Xunit;
using Movies.Contracts;

namespace Movies.Tests
{
	[Collection("default")]
	public class UnitTest1(ClusterFixture fixture)
	{
		private readonly TestCluster _cluster = fixture.Cluster;
		
		[Fact]
		public async Task Test1()
		{
			var take = 5;
			var listGrain = _cluster.GrainFactory.GetGrain<IListMoviesGrain>($"top {take}");

			var movies = await listGrain.GetHighestRate(take);
		}
	}
}

