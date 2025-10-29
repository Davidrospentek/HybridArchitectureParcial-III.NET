namespace Application.Exceptions
{
    /// <summary>
    /// Excepción para errores de lógica de negocio
    /// </summary>
    public class BussinessException : Exception
    {
        public BussinessException(string message)
            : base(message)
        {
        }

        public BussinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}