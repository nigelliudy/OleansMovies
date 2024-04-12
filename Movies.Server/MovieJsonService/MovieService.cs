using System;
using System.IO;
using System.Threading;
using Movies.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Movies.Server.MovieJsonService
{
	public static class MovieService
	{
		/// <summary>
		/// Pre-loads database data.
		/// </summary>
		public static async Task Configure(MoviesContext context, string filename, CancellationToken cancellation)
		{
			Console.WriteLine("Loading data file '{0}'.", filename);

			await using var fileStream = File.OpenRead(filename);
			using var streamReader = new StreamReader(fileStream);
			using var jsonReader = new JsonTextReader(streamReader);
			// The data is a property "movies" in the json root, 
			// which should be the first array encountered.
			while (jsonReader.TokenType != JsonToken.StartArray) await jsonReader.ReadAsync(cancellation);

			var jsonSerializer = new JsonSerializer();
			jsonSerializer.Converters.Add(new StringArrayConverter());
			// Read token by token from the stream
			while (await jsonReader.ReadAsync(cancellation))
			{
				if (jsonReader.TokenType != JsonToken.StartObject) continue;

				var theObject = jsonSerializer.Deserialize<MovieJsonModel>(jsonReader);
				if (theObject is not null)
				{
					var movieObject = theObject.MapToMovie();
					if (context.Movies.Find(movieObject.Id) == null)
					{
						Console.WriteLine($"Loaded movie, {movieObject.Id} : {movieObject.Name}");
						await context.Movies.AddAsync(movieObject, cancellation);
					}
					else
						Console.WriteLine($"Duplicate found, {movieObject.Id} : {movieObject.Name}");
				}
			}

			await context.SaveChangesAsync(cancellation);
		}
	}
}