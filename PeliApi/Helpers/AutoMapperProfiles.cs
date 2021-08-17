using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliApi.DTOs;
using PeliApi.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles(GeometryFactory geometryFactory)
		{
			CreateMap<Genero, GeneroDTO>().ReverseMap();
			CreateMap<GeneroCreacionDTO, Genero>();

			CreateMap<SalaDeCine, SalaDeCineDTO>()
				.ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
				.ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));


			CreateMap<Review, ReviewDTO>()
				.ForMember(x => x.NombreUsuario, x => x.MapFrom(y => y.Usuario.UserName));

			CreateMap<ReviewDTO, Review>();
			CreateMap<ReviewCreacionDTO, Review>();



			CreateMap<IdentityUser, UsuarioDTO>();

			CreateMap<SalaDeCineDTO, SalaDeCine>()
				.ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
				geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

			CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
				 .ForMember(x => x.Ubicacion, x => x.MapFrom(y =>
				geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));


			CreateMap<Actor, ActorDTO>().ReverseMap();
			CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.Foto, options => options.Ignore());
			CreateMap<ActorPatchDTO, Actor>().ReverseMap();

			CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
			CreateMap<PeliculaCreacionDTO, Pelicula>()
				.ForMember(x => x.Poster, options => options.Ignore())
				.ForMember(x => x.PeliculasGenero, options => options.MapFrom(MapPeliculaGenero))
				.ForMember(x => x.PeliculasActor, options => options.MapFrom(MapPeliculaActor));
			CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();

			CreateMap<Pelicula, PeliculaDetalleDTO>()
				.ForMember(x => x.Generos, options => options.MapFrom(MapPeliculasGenero))
				.ForMember(x => x.Actores, options => options.MapFrom(MapPeliculaActores));


		}

		private List<ActorPeliculaDetalleDTO> MapPeliculaActores(Pelicula pelicula, PeliculaDetalleDTO peliculaDetalleDTO)
		{
			var resultado = new List<ActorPeliculaDetalleDTO>();
			if (pelicula.PeliculasActor == null) { return resultado; }
			foreach (var actorPelicula in pelicula.PeliculasActor)
			{
				resultado.Add(new ActorPeliculaDetalleDTO() { 
					ActorId = actorPelicula.ActorId, 
					Personaje = actorPelicula.Personaje,
					NombrePersona = actorPelicula.Actor.Nombre
				});
			}
			return resultado;
		}

		private List<GeneroDTO> MapPeliculasGenero(Pelicula pelicula, PeliculaDetalleDTO peliculaDetalleDTO)
		{
			var resultado = new List<GeneroDTO>();
			if(pelicula.PeliculasGenero == null) { return resultado; }
			foreach (var generoPelicula	in pelicula.PeliculasGenero)
			{
				resultado.Add(new GeneroDTO() { Id = generoPelicula.GeneroId, Nombre = generoPelicula.Genero.Nombre });
			}
			return resultado;
		}

		private List<PeliculasGenero> MapPeliculaGenero(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
		{
			var resultado = new List<PeliculasGenero>();
			if(peliculaCreacionDTO.GenerosIDs == null)
			{
				return resultado;
			}
			foreach (var id in peliculaCreacionDTO.GenerosIDs)
			{
				resultado.Add(new PeliculasGenero() { GeneroId = id });
			}
			return resultado;
		}

		private List<PeliculasActor> MapPeliculaActor(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
		{
			var resultado = new List<PeliculasActor>();
			if (peliculaCreacionDTO.Actores == null)
			{
				return resultado;
			}
			foreach (var actor in peliculaCreacionDTO.Actores)
			{
				resultado.Add(new PeliculasActor() { ActorId = actor.ActorId, Personaje=actor.Personaje });
			}
			return resultado;
		}
	}
}
