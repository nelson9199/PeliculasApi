using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace src.PeliculasApi.Validations
{
    public class TipoArchivoValidation : ValidationAttribute
    {
        private readonly string[] tipoValidos;
        public TipoArchivoValidation(string[] tipoValidos)
        {
            this.tipoValidos = tipoValidos;

        }

        public TipoArchivoValidation(GrupoTipoArchivo grupoTipoArchivo)
        {
            if (grupoTipoArchivo == GrupoTipoArchivo.Imagen)
            {
                tipoValidos = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            if (!tipoValidos.Contains(formFile.ContentType))
            {
                return new ValidationResult($"El tipo del archivo debe ser uno de los siguientes: {string.Join(", ", tipoValidos)}");
            }

            return ValidationResult.Success;
        }
    }
}