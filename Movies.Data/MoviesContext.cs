using System;
using Microsoft.EntityFrameworkCore;

namespace Movies.Data
{
	/// <summary>
	/// The database context that coordinates CRUD operations.
	/// </summary>
	public class MoviesContext : DbContext
	{
		public DbSet<Movie> Movies { get; set; } = null!;

		public MoviesContext(DbContextOptions<MoviesContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Movie>()
				.HasKey(h => h.Id);

			modelBuilder.Entity<Movie>()
				.Property(p => p.Genres)
				.HasConversion(
					h => string.Join("|", h),
					h => h.Split(new char ['|'], StringSplitOptions.RemoveEmptyEntries));

			base.OnModelCreating(modelBuilder);
		}
	}
}