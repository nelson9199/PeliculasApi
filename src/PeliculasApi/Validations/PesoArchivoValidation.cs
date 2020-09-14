using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace src.PeliculasApi.Validations
{
    public class PesoArchivoValidation : ValidationAttribute
    {
        private readonly int pesoMaxEnMB;
        public PesoArchivoValidation(int pesoMaxEnMB)
        {
            this.pesoMaxEnMB = pesoMaxEnMB;

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

            if (formFile.Length > pesoMaxEnMB * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaxEnMB} MB");
            }

            return ValidationResult.Success;
        }
    }
}