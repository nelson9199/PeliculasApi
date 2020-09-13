using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models.Dtos
{
    public class GeneroCreateDto
    {
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}