﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Entidades
{
	public class Genero
	{
		public int Id { get; set; }
		[Required]
		[StringLength(40)]
		public string Nombre { get; set; }
		public List<PeliculasGenero> PeliculasGenero { get; set; }
	}
}
