using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models
{
    public class Genero
    {
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
    }
}