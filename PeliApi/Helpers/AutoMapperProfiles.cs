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
		}
	}
}
