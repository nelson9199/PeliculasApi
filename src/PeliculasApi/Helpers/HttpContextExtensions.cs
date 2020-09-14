using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace src.PeliculasApi.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext,
        IQueryable<T> queryable, int cantidadRegistrosPorPaginas)
        {
            double cantidadRegistros = await queryable.CountAsync();

            double cantidadPaginas = Math.Ceiling(cantidadRegistros / cantidadRegistrosPorPaginas);

            httpContext.Response.Headers.Add("cantidadPaginas", cantidadPaginas.ToString());
        }
    }
}