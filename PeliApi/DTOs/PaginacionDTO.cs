using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.DTOs
{
	public class PaginacionDTO
	{
		public int pagina { get; set; } = 1;

		private int cantidadRegistrosPorPagina = 10;

		private readonly int cantidadMayimaRegistrosPorPaginas = 50;


		//sirve para paginar en un mayimo de 50 o si es menor el q decida el usuario
		public int CantidadRegistrosPorPagina {
			get => cantidadRegistrosPorPagina;
			set
			{
				cantidadRegistrosPorPagina = (value > cantidadMayimaRegistrosPorPaginas) ? cantidadMayimaRegistrosPorPaginas : value;
			}
		}
	}
}
