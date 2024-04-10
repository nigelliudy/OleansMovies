namespace Movies.Data
{
	public class Movie
	{
		public int Id { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string[] Genres { get; set; }
		public decimal Rate { get; set; }
		public string Length { get; set; }
		public string Image { get; set; }
	}
}