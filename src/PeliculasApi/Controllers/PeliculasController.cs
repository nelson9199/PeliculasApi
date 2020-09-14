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
using src.PeliculasApi.Data;
using src.PeliculasApi.Models;
using src.PeliculasApi.Models.Dtos;
using src.PeliculasApi.Services;

namespace PeliculasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";
        public PeliculasController(ApplicationDbContext context, IMapper mapper,
        IAlmacenadorArchivos almacenadorArchivos)
        {
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeliculaDto>>> GetAsync()
        {
            var peliculasDb = await context.Peliculas.ToListAsync();

            return mapper.Map<List<PeliculaDto>>(peliculasDb);
        }

        [HttpGet("{id}", Name = "ObtenerPelicula")]
        public async Task<ActionResult<PeliculaDto>> GetAsync(int id)
        {
            var peliculaDb = await context.Peliculas.FindAsync(id);

            if (peliculaDb == null)
            {
                return NotFound();
            }

            return mapper.Map<PeliculaDto>(peliculaDb);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] PeliculaCreateDto peliculaCreateDto)
        {
            var peliculaCreateDb = mapper.Map<Pelicula>(peliculaCreateDto);

            if (peliculaCreateDto.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreateDto.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreateDto.Poster.FileName);
                    peliculaCreateDb.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, peliculaCreateDto.Poster.ContentType);
                }
            }

            AsignarOrdenActores(peliculaCreateDb);

            context.Peliculas.Add(peliculaCreateDb);

            await context.SaveChangesAsync();

            var peliculaDto = mapper.Map<PeliculaDto>(peliculaCreateDb);

            return new CreatedAtRouteResult("ObtenerPelicula", new { id = peliculaDto.Id }, peliculaDto);
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromForm] PeliculaCreateDto peliculaUpdateDto)
        {
            var peliculaUpdateDb = await context.Peliculas
            .Include(x => x.PeliculasActores)
            .Include(x => x.PeliculasGeneros)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (peliculaUpdateDb == null)
            {
                return NotFound();
            }

            mapper.Map(peliculaUpdateDto, peliculaUpdateDb);

            if (peliculaUpdateDto.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaUpdateDto.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaUpdateDto.Poster.FileName);
                    peliculaUpdateDb.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, peliculaUpdateDb.Poster, peliculaUpdateDto.Poster.ContentType);
                }
            }

            AsignarOrdenActores(peliculaUpdateDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<PeliculaPatchDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var peliculaDb = await context.Peliculas.FindAsync(id);

            if (peliculaDb == null)
            {
                return NotFound();
            }

            var peliculaPatchDto = mapper.Map<PeliculaPatchDto>(peliculaDb);

            patchDocument.ApplyTo(peliculaPatchDto, ModelState);

            var esValido = TryValidateModel(peliculaPatchDto);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(peliculaPatchDto, peliculaDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existe = await context.Peliculas.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Pelicula() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}