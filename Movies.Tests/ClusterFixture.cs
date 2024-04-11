using Orleans.TestingHost;

namespace Movies.Tests
{
	public class ClusterFixture : IDisposable
	{
		public TestCluster Cluster { get; } = new TestClusterBuilder().Build();

		public ClusterFixture() => Cluster.Deploy();

		void IDisposable.Dispose() => Cluster.StopAllSilos();
	}
}