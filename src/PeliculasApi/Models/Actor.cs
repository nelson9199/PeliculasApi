using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }

        public List<PeliculasActores> PeliculasActores { get; set; }
    }
}