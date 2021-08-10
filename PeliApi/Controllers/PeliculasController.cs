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
using PeliApi.Servicios;

namespace PeliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly AplicationDbContext _context;
		private readonly IMapper mapper;
		private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";

		public PeliculasController(AplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
			this.mapper = mapper;
			this.almacenadorArchivos = almacenadorArchivos;
		}

        // GET: api/Peliculas
        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> GetPeliculas()
        {
            var peliculas = await _context.Peliculas.ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        // GET: api/Peliculas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaDTO>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return mapper.Map<PeliculaDTO>(pelicula);
        }

        // PUT: api/Peliculas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id,[FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {


            var peliculaDb = await _context.Peliculas
                .Include(x=>x.PeliculasActor)
                .Include(x=>x.PeliculasGenero)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (peliculaDb == null)
            {
                return NotFound();
            }

            peliculaDb = mapper.Map(peliculaCreacionDTO, peliculaDb);



            //Para Imagenes
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDb.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, peliculaDb.Poster, peliculaCreacionDTO.Poster.ContentType);

                }
            }

            AsignarOrdenActores(peliculaDb);

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



        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchActor(int id, [FromBody] JsonPatchDocument<PeliculaPatchDTO> patchDocument)
        {
            //el usuario no envio la data como debia
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var entidadDb = await _context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);

            if (entidadDb == null)
            {
                return NotFound();
            }

            var entidadDTO = mapper.Map<PeliculaPatchDTO>(entidadDb);

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



        // POST: api/Peliculas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostPelicula([FromForm]PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);



			//Para Imagenes
			if (peliculaCreacionDTO.Poster != null)
			{
				using (var memoryStream = new MemoryStream())
				{
					await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
					var contenido = memoryStream.ToArray();
					var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
					pelicula.Poster = await almacenadorArchivos.GuardarAchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType);

				}
			}

            AsignarOrdenActores(pelicula);
			_context.Peliculas.Add(pelicula);
			await _context.SaveChangesAsync();

			var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

			return CreatedAtAction("GetPelicula", new { id = peliculaDTO.Id }, peliculaDTO);
		}

        private void AsignarOrdenActores(Pelicula pelicula)
		{
            if(pelicula.PeliculasActor != null)
			{
				for (int i = 0; i < pelicula.PeliculasActor.Count; i++)
				{
                    pelicula.PeliculasActor[i].Orden = i;
				}
			}
		}

        // DELETE: api/Peliculas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePelicula(int id)
        {
            var existe = await _context.Peliculas.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            _context.Remove(new Pelicula() { Id = id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }
    }
}
