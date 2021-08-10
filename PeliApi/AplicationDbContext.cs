using Microsoft.EntityFrameworkCore;
using PeliApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi
{
	public class AplicationDbContext : DbContext
	{
		public AplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PeliculasActor>().HasKey(x => new { x.ActorId, x.PeliculaId });

			modelBuilder.Entity<PeliculasGenero>().HasKey(x => new { x.GeneroId, x.PeliculaId });

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Genero> Generos { get; set; }
		public DbSet<Actor> Actores { get; set; }
		public DbSet<Pelicula> Peliculas { get; set; }

		public DbSet<PeliculasActor> PeliculasActores { get; set; }

		public DbSet<PeliculasGenero> PeliculasGeneros { get; set; }
	}
}
