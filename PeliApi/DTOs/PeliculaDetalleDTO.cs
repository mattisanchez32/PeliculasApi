using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.DTOs
{
	public class PeliculaDetalleDTO: PeliculaDTO
	{
		public List<GeneroDTO> Generos { get; set; }
		public List<ActorPeliculaDetalleDTO> Actores { get; set; }
	}
}
