using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeliApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PeliApi
{
	public class AplicationDbContext : IdentityDbContext
	{
		public AplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PeliculasActor>().HasKey(x => new { x.ActorId, x.PeliculaId });

			modelBuilder.Entity<PeliculasGenero>().HasKey(x => new { x.GeneroId, x.PeliculaId });

			modelBuilder.Entity<PeliculasSalasDeCine>()
				.HasKey(x => new { x.PeliculaId, x.SalaDeCineId });

			SeedData(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private void SeedData(ModelBuilder modelBuilder)
		{
			var rolAdminId = "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d";
			var usuarioAdminId = "5673b8cf-12de-44f6-92ad-fae4a77932ad";

			var rolAdmin = new IdentityRole()
			{
				Id = rolAdminId,
				Name = "Admin",
				NormalizedName = "Admin"
			};

			var passwordHasher = new PasswordHasher<IdentityUser>();

			var username = "matisanchez@hotmail.com";

			var usuarioAdmin = new IdentityUser()
			{
				Id = usuarioAdminId,
				UserName = username,
				NormalizedUserName = username,
				Email = username,
				NormalizedEmail = username,
				PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
			};


			//modelBuilder.Entity<IdentityUser>()
			//	.HasData(usuarioAdmin);

			//modelBuilder.Entity<IdentityRole>()
			//	.HasData(rolAdmin);

			//modelBuilder.Entity<IdentityUserClaim<string>>()
			//	.HasData(new IdentityUserClaim<string>()
			//	{
			//		Id = 1,
			//		ClaimType = ClaimTypes.Role,
			//		UserId = usuarioAdminId,
			//		ClaimValue = "Admin"
			//	});


		}


		public DbSet<Genero> Generos { get; set; }
		public DbSet<Actor> Actores { get; set; }
		public DbSet<Pelicula> Peliculas { get; set; }

		public DbSet<PeliculasActor> PeliculasActores { get; set; }

		public DbSet<PeliculasGenero> PeliculasGeneros { get; set; }

		public DbSet<SalaDeCine> SalasDeCine { get; set; }
		public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }

		public DbSet<Review> Reviews{ get; set; }
	}
}
