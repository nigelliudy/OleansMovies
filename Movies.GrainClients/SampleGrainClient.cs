using System.IO;
using Movies.Contracts;
using Orleans;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Movies.GrainClients
{
	public class SampleGrainClient : ISampleGrainClient
	{
		private readonly IGrainFactory _grainFactory;

		public SampleGrainClient(
			IGrainFactory grainFactory
		)
		{
			_grainFactory = grainFactory;
		}

		public Task<SampleDataModel> Get(string id)
		{
			var grain = _grainFactory.GetGrain<ISampleGrain>(id);
			return grain.Get();
		}

		public Task Set(string key, string name)
		{
			var grain = _grainFactory.GetGrain<ISampleGrain>(key);
			return grain.Set(name);
		}

		public async Task Configure(string filename)
		{
			using var fileStream = File.OpenRead(filename);
			using var streamReader = new StreamReader(fileStream);
			using var jsonReader = new JsonTextReader(streamReader);
			// The data is a property "movies" in the json root, 
			// which should be the first array encountered.
			while (jsonReader.TokenType != JsonToken.StartArray) await jsonReader.ReadAsync();

			var jsonSerializer = new JsonSerializer();
			// Read token by token from the stream
			while (await jsonReader.ReadAsync())
			{
				if (jsonReader.TokenType != JsonToken.StartObject) continue;

				var theObject = jsonSerializer.Deserialize<SampleDataModel>(jsonReader);
				// Console.WriteLine($"SampleDataModel {theObject.Id} : {theObject.Name}");
				await Set(theObject.Id, theObject.Name);
			}
		}
	}
}