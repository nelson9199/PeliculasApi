using System.IO;
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
using src.PeliculasApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using src.PeliculasApi.Helpers;

namespace PeliculasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";
        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.almacenadorArchivos = almacenadorArchivos;
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetAsync([FromQuery] PaginacionDto paginacionDto)
        {
            var queryable = context.Actores.AsQueryable();

            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDto.CantidadRegistrosPorPagina);

            var actoresDb = await queryable.Paginar(paginacionDto).ToListAsync();

            return mapper.Map<List<ActorDto>>(actoresDb);
        }

        [HttpGet("{id}", Name = "ObtenerActor")]
        public async Task<ActionResult<ActorDto>> GetAsync(int id)
        {
            var actorDb = await context.Actores.FindAsync(id);

            if (actorDb == null)
            {
                return NotFound();
            }

            return mapper.Map<ActorDto>(actorDb);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromForm] ActorCreateDto actorCreateDto)
        {
            var actorCreateDb = mapper.Map<Actor>(actorCreateDto);

            if (actorCreateDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreateDto.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreateDto.Foto.FileName);
                    actorCreateDb.Foto = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, actorCreateDto.Foto.ContentType);
                }
            }

            context.Actores.Add(actorCreateDb);

            await context.SaveChangesAsync();

            var actorDto = mapper.Map<ActorDto>(actorCreateDb);

            return new CreatedAtRouteResult("ObtenerActor", new { id = actorDto.Id }, actorDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromForm] ActorCreateDto actorUpdateDto)
        {
            var actorUpdateDb = await context.Actores.FindAsync(id);

            if (actorUpdateDb == null)
            {
                return NotFound();
            }

            mapper.Map(actorUpdateDto, actorUpdateDb);

            if (actorUpdateDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorUpdateDto.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorUpdateDto.Foto.FileName);
                    actorUpdateDb.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, actorUpdateDb.Foto, actorUpdateDto.Foto.ContentType);
                }
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync(int id, [FromBody] JsonPatchDocument<ActorPatchDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var actorDb = await context.Actores.FindAsync(id);

            if (actorDb == null)
            {
                return NotFound();
            }

            var actorPatchDto = mapper.Map<ActorPatchDto>(actorDb);

            patchDocument.ApplyTo(actorPatchDto, ModelState);

            var esValido = TryValidateModel(actorPatchDto);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(actorPatchDto, actorDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var existe = await context.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Actor() { Id = id });

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}