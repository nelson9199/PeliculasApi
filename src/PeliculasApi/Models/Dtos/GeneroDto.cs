using System.ComponentModel.DataAnnotations;

namespace src.PeliculasApi.Models.Dtos
{
    public class GeneroDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
    }
}