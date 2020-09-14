using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.PeliculasApi.Helpers;
using src.PeliculasApi.Validations;

namespace src.PeliculasApi.Models.Dtos
{
    public class PeliculaCreateDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }

        [PesoArchivoValidation(pesoMaxEnMB: 4)]
        [TipoArchivoValidation(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public List<int> GenerosId { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<ActorPeliculasCreateDto>))]
        public List<ActorPeliculasCreateDto> Actores { get; set; }
    }
}