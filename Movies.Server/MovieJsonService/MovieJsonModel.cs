﻿using System;
using Movies.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Movies.Server.MovieJsonService
{
	/// <summary>
	/// Represents a JObject for each movie entry.
	/// </summary>
	public class MovieJsonModel
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[JsonConverter(typeof(StringArrayConverter))]
		public string[] Genres { get; set; }

		public decimal Rate { get; set; }
		public string Length { get; set; }
		public string Img { get; set; }
	}

	/// <summary>
	/// Map from json movie model to application's movie model.
	/// </summary>
	public static class MovieJsonModelExtensions
	{
		public static Movie MapToMovie(this MovieJsonModel model) =>
			new Movie
			{
				Id = model.Id,
				Key = model.Key,
				Name = model.Name,
				Description = model.Description,
				Genres = model.Genres,
				Rate = model.Rate,
				Length = model.Length,
				Image = model.Img
			};
	}

	/// <summary>
	/// Converts JArray into string [].
	/// </summary>
	public class StringArrayConverter : JsonConverter
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartArray)
			{
				var obj = JArray.Load(reader);
				if (obj is null)
					return Array.Empty<string>();
				return obj.ToObject<string[]>();
			}

			return Array.Empty<string>();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
			throw new NotImplementedException("Not implemented.");

		public override bool CanWrite => false;

		public override bool CanConvert(Type objectType) => objectType == typeof(string[]);
	}
}