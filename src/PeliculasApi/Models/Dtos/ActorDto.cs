using System;
using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models.Dtos
{
    public class ActorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }
    }
}