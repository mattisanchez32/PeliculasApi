using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliApi.DTOs;
using PeliApi.Entidades;
using PeliApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliApi.Controllers
{
	//Sirve para reutilizar codigo
	[Route("api/[controller]")]
	[ApiController]
	public class CustomBaseController : ControllerBase
	{
		private readonly AplicationDbContext context;
		private readonly IMapper mapper;

		public CustomBaseController(AplicationDbContext context, IMapper mapper)
		{
			this.context = context;
			this.mapper = mapper;
		}

		protected async Task<List<TDTO>> Get<TEntidad, TDTO>() where TEntidad : class
		{
			//sirve para trabajar con cualquier entidad q yo quiera trabajar
			var entidades = await context.Set<TEntidad>().AsNoTracking().ToListAsync();
			var dtos = mapper.Map<List<TDTO>>(entidades);
			return dtos;
		}

		//protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO)
		//	where TEntidad : class
		//{
		//	var queryable = context.Set<TEntidad>().AsQueryable();
		//	await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
		//	//cambio _context.Acotores por queryable.Paginar para la paginacion
		//	var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
		//	return mapper.Map<List<TDTO>>(entidades);
		//}


		protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO)
		   where TEntidad : class
		{
			var queryable = context.Set<TEntidad>().AsQueryable();
			return await Get<TEntidad, TDTO>(paginacionDTO, queryable);
		}


		protected async Task<List<TDTO>> Get<TEntidad, TDTO>(PaginacionDTO paginacionDTO,
			IQueryable<TEntidad> queryable)
			where TEntidad : class
		{
			await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
			var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
			return mapper.Map<List<TDTO>>(entidades);
		}


		protected async Task<ActionResult <TDTO>> Get<TEntidad, TDTO>(int id) where TEntidad : class, IId
		{
			//Se crea una entidad para solventar el problema del x.Id llamada IId en entidades
			var entidad = await context.Set<TEntidad>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

			if(entidad == null)
			{
				return NotFound();
			}

			return mapper.Map<TDTO>(entidad);
		}

		protected async Task<ActionResult> Post<TCreacion,TEntidad,TLectura>
			(TCreacion creacionDTO, string nombreRuta) where TEntidad :class, IId
		{
			var entidad = mapper.Map<TEntidad>(creacionDTO);
			context.Add(entidad);
			await context.SaveChangesAsync();

			var dtoLectura = mapper.Map<TLectura>(entidad);

			return CreatedAtAction(nombreRuta, new { id = entidad.Id }, dtoLectura);
		}

		protected async Task<ActionResult> Put<TCreacion, TEntidad>
			(int id, TCreacion creacionDTO) where TEntidad : class, IId
		{
			var entidad = mapper.Map<TEntidad>(creacionDTO);
			entidad.Id = id;
			if (id != entidad.Id)
			{
				return BadRequest();
			}

			context.Entry(entidad).State = EntityState.Modified;
			await context.SaveChangesAsync();

			return NoContent();
		}


		protected async Task<ActionResult> Patch<TEntidad, TDTO>(int id, JsonPatchDocument<TDTO> patchDocument)
			where TDTO : class
			where TEntidad : class, IId
		{
			if (patchDocument == null)
			{
				return BadRequest();
			}

			var entidadDB = await context.Set<TEntidad>().FirstOrDefaultAsync(x => x.Id == id);

			if (entidadDB == null)
			{
				return NotFound();
			}

			var dto = mapper.Map<TDTO>(entidadDB);

			patchDocument.ApplyTo(dto, ModelState);

			var isValid = TryValidateModel(dto);

			if (!isValid)
			{
				return BadRequest(ModelState);
			}

			mapper.Map(dto, entidadDB);

			await context.SaveChangesAsync();

			return NoContent();
		}



		protected async Task<ActionResult> Delete<TEntidad>(int id) where TEntidad : class , IId, new()
		{
			var existe = await context.Set<TEntidad>().AnyAsync(x => x.Id == id);
			if (!existe)
			{
				return NotFound();
			}

			context.Remove(new TEntidad() { Id = id });
			await context.SaveChangesAsync();

			return NoContent();
		}
	}
}
