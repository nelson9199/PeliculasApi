namespace src.PeliculasApi.Models.Dtos
{
    public class PaginacionDto
    {
        public int Pagina { get; set; } = 1;
        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaxDeRegistrosPorPagina = 50;
        public int CantidadRegistrosPorPagina
        {
            get => cantidadRegistrosPorPagina;

            set
            {
                cantidadRegistrosPorPagina =
                (value > cantidadMaxDeRegistrosPorPagina) ? cantidadMaxDeRegistrosPorPagina : value;
            }
        }
    }
}