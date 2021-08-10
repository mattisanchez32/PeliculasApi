using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Entidades
{
	public class Pelicula
	{
		public int Id { get; set; }
		[Required]
		[StringLength(300)]
		public string Titulo { get; set; }
		public bool EnCines { get; set; }
		public DateTime FechaEstreno { get; set; }
		public string Poster { get; set; }

		public List<PeliculasActor> PeliculasActor{ get; set; }
		public List<PeliculasGenero> PeliculasGenero{ get; set; }

	}
}
