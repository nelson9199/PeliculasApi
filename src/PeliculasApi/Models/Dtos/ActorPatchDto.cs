using System;
using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models.Dtos
{
    public class ActorPatchDto
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}