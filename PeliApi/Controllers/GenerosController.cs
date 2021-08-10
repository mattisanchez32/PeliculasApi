using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliApi;
using PeliApi.DTOs;
using PeliApi.Entidades;

namespace PeliApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly AplicationDbContext _context;
		private readonly IMapper mapper;

		public GenerosController(AplicationDbContext context, IMapper mapper)
        {
            _context = context;
			this.mapper = mapper;
		}

        // GET: api/Generos
        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> GetGeneros()
        {
            var entidades = await _context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        // GET: api/Generos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneroDTO>> GetGenero(int id)
        {

            var genero = await _context.Generos.FindAsync(id);

            if (genero == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<GeneroDTO>(genero);

            return dto;
        }

        // PUT: api/Generos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenero(int id,[FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            if (id != entidad.Id)
            {
                return BadRequest();
            }

            _context.Entry(entidad).State = EntityState.Modified;

           // try
            //{
                await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
              //  if (!GeneroExists(id))
               // {
               //     return NotFound();
               // }
               // else
               // {
               //     throw;
               // }
           // }

            return NoContent();
        }

        // POST: api/Generos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostGenero([FromBody]GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            _context.Generos.Add(entidad);
            await _context.SaveChangesAsync();

            var generoDTO = mapper.Map<GeneroDTO>(entidad);

            return CreatedAtAction("GetGenero", new { id = generoDTO.Id }, generoDTO);
        }

        // DELETE: api/Generos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGenero(int id)
        {

            //var genero = await _context.Generos.FindAsync(id);
            //if (genero == null)
            //{
            //    return NotFound();
            //}
            var existe = await _context.Generos.AnyAsync(x => x.Id == id);
			if (!existe)
			{
                return NotFound();
			}

            _context.Remove(new Genero() { Id=id });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeneroExists(int id)
        {
            return _context.Generos.Any(e => e.Id == id);
        }
    }
}
