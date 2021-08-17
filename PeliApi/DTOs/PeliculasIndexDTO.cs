using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.DTOs
{
	public class PeliculasIndexDTO
	{
		public List<PeliculaDTO> FuturosEstrenos { get; set; }
		public List<PeliculaDTO> PeliculasEnCines { get; set; }

	}
}
