using AutoMapper;
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
		public AutoMapperProfiles()
		{
			CreateMap<Genero, GeneroDTO>().ReverseMap();
			CreateMap<GeneroCreacionDTO, Genero>();

			CreateMap<Actor, ActorDTO>().ReverseMap();
			CreateMap<ActorCreacionDTO, Actor>().ForMember(x => x.Foto, options => options.Ignore());
			CreateMap<ActorPatchDTO, Actor>().ReverseMap();

			CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
			CreateMap<PeliculaCreacionDTO, Pelicula>()
				.ForMember(x => x.Poster, options => options.Ignore())
				.ForMember(x => x.PeliculasGenero, options => options.MapFrom(MapPeliculaGenero))
				.ForMember(x => x.PeliculasActor, options => options.MapFrom(MapPeliculaActor));
			CreateMap<PeliculaPatchDTO, Pelicula>().ReverseMap();


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
