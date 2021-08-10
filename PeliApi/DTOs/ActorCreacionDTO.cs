using Microsoft.AspNetCore.Http;
using PeliApi.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.DTOs
{
	public class ActorCreacionDTO
	{
		[Required]
		[StringLength(120)]
		public string Nombre { get; set; }
		public DateTime FechaNacimiento { get; set; }
		[PesoArchivoValidacion(pesoMaximoEnMegaBytes: 4)]
		[TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
		public IFormFile Foto { get; set; }
	}
}
