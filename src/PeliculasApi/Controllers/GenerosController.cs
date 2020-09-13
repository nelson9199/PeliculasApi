using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.PeliculasApi.Data;
using src.PeliculasApi.Models;
using src.PeliculasApi.Models.Dtos;

namespace PeliculasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneroDto>>> GetAsync()
        {
            var generoDb = await context.Generos.ToListAsync();

            var generoDto = mapper.Map<List<GeneroDto>>(generoDb);

            return generoDto;
        }

        [HttpGet("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDto>> GetAsync(int id)
        {
            var generoDb = await context.Generos.FindAsync(id);

            if (generoDb == null)
            {
                return NotFound();
            }

            var generoDto = mapper.Map<GeneroDto>(generoDb);

            return generoDto;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] GeneroCreateDto generoCreateDto)
        {
            var generoCreateDb = mapper.Map<Genero>(generoCreateDto);

            context.Generos.Add(generoCreateDb);

            await context.SaveChangesAsync();

            var generoDto = mapper.Map<GeneroDto>(generoCreateDb);

            return new CreatedAtRouteResult("ObtenerGenero", new { id = generoDto.Id }, generoDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] GeneroCreateDto generoUpdateDto)
        {
            var generoUpdateDb = await context.Generos.FindAsync(id);

            if (generoUpdateDb == null)
            {
                return NotFound();
            }
            mapper.Map(generoUpdateDto, generoUpdateDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Genero() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}