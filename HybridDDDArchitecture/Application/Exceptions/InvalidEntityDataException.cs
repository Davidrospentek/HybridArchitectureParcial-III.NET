using FluentValidation.Results;

namespace Application.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando una entidad no cumple con las reglas de validación de dominio
    /// </summary>
    public class InvalidEntityDataException : Exception
    {
        public IList<ValidationFailure> Errors { get; }

        public InvalidEntityDataException(IList<ValidationFailure> errors)
            : base("La entidad contiene datos inválidos")
        {
            Errors = errors;
        }

        /// <summary>
        /// Obtiene todos los mensajes de error concatenados
        /// </summary>
        public string GetErrorMessages()
        {
            return string.Join("; ", Errors.Select(e => e.ErrorMessage));
        }
    }
}
