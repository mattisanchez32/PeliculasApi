using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliApi;
using PeliApi.DTOs;
using PeliApi.Entidades;
using PeliApi.Helpers;
using PeliApi.Servicios;

namespace PeliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly AplicationDbContext _context;
		private readonly IMapper mapper;
		private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";

		public ActoresController(AplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
			this.mapper = mapper;
			this.almacenadorArchivos = almacenadorArchivos;
		}

        // GET: api/Actores
        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> GetActores([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            //cambio _context.Acotores por queryable.Paginar para la paginacion
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(entidades);
        }

        // GET: api/Actores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActorDTO>> GetActor(int id)
        {
            var actor = await _context.Actores.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<ActorDTO>(actor);

            return dto;
        }

        // PUT: api/Actores/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, [FromForm]ActorCreacionDTO actorCreacionDTO)
        {
            //var entidad = mapper.Map<Actor>(actorCreacionDTO);
            //entidad.Id = id;
            //if (id != entidad.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(entidad).State = EntityState.Modified;

            var actorDb = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

			if (actorDb == null)
			{
                return NotFound();
			}

            actorDb = mapper.Map(actorCreacionDTO, actorDb);



            //Para Imagenes
            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDb.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, actorDb.Foto, actorCreacionDTO.Foto.ContentType);

                }
            }


            //try
            //{
            await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ActorExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/Actores
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostActor([FromForm]ActorCreacionDTO actorCreacionDTO)
        {
            var entidad = mapper.Map<Actor>(actorCreacionDTO);

            //Para Imagenes
            if(actorCreacionDTO.Foto != null)
			{
                using(var memoryStream = new MemoryStream())
				{
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    entidad.Foto = await almacenadorArchivos.GuardarAchivo(contenido, extension, contenedor, actorCreacionDTO.Foto.ContentType);

				}
			}


            _context.Actores.Add(entidad);
            await _context.SaveChangesAsync();

            var actorDTO = mapper.Map<ActorDTO>(entidad);

            return CreatedAtAction("GetActor", new { id = actorDTO.Id }, actorDTO);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchActor(int id, [FromBody] JsonPatchDocument<ActorPatchDTO> patchDocument)
		{
            //el usuario no envio la data como debia
            if(patchDocument == null)
			{
                return BadRequest();
			}

            var entidadDb = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if(entidadDb == null)
			{
                return NotFound();
			}

            var entidadDTO = mapper.Map<ActorPatchDTO>(entidadDb);

            patchDocument.ApplyTo(entidadDTO, ModelState);

            var esValido = TryValidateModel(entidadDTO);

			if (!esValido)
			{
                return BadRequest(ModelState);
			}

            mapper.Map(entidadDTO, entidadDb);

            await _context.SaveChangesAsync();

            return NoContent();

		}

        // DELETE: api/Actores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            var existe = await _context.Actores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            _context.Remove(new Actor() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActorExists(int id)
        {
            return _context.Actores.Any(e => e.Id == id);
        }
    }
}
