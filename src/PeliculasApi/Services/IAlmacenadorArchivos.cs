using System.Threading.Tasks;

namespace src.PeliculasApi.Services
{
    public interface IAlmacenadorArchivos
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, string contenedor, string ruta, string contentType);
        Task<string> GuardarArchivo(byte[] contenido, string extension, string contenedor, string contentType);
        Task BorrarArchivo(string ruta, string contenedor);

    }
}