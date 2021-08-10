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

		public DbSet<Genero> Generos { get; set; }
		public DbSet<Actor> Actores { get; set; }
	}
}
