using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using src.PeliculasApi.Validations;

namespace src.PeliculasApi.Models.Dtos
{
    public class ActorCreateDto
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }

        [PesoArchivoValidation(pesoMaxEnMB: 2)]
        [TipoArchivoValidation(GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}