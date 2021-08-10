using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Entidades
{
	public class PeliculasGenero
	{
		public int GeneroId { get; set; }
		public int PeliculaId { get; set; }
		public Genero Genero { get; set; }
		public Pelicula Pelicula { get; set; }
	}
}
