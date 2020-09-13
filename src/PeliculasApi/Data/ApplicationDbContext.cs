using Microsoft.EntityFrameworkCore;
using src.PeliculasApi.Models;

namespace src.PeliculasApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genero> Generos { get; set; }
    }
}