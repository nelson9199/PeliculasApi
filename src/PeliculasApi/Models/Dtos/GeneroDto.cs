using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models.Dtos
{
    public class GeneroDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}