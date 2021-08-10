using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliApi.Helpers;
using PeliApi.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.DTOs
{
	public class PeliculaCreacionDTO
	{
		
		[Required]
		[StringLength(300)]
		public string Titulo { get; set; }
		public bool EnCines { get; set; }
		public DateTime FechaEstreno { get; set; }
		[PesoArchivoValidacion(pesoMaximoEnMegaBytes:4)]
		[TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
		public IFormFile Poster { get; set; }
		[ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
		public List<int> GenerosIDs { get; set; }
		[ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculasCreacionDTO>>))]
		public List<ActorPeliculasCreacionDTO> Actores { get; set; }
	}
}
