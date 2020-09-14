using Microsoft.EntityFrameworkCore;
using src.PeliculasApi.Models;

namespace src.PeliculasApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PeliculasActores>()
            .HasKey(x => new { x.PeliculaId, x.ActorId });

            builder.Entity<PeliculasGeneros>()
            .HasKey(x => new { x.PeliculaId, x.GeneroId });

            base.OnModelCreating(builder);
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
    }
}