namespace Application.Services
{
    /// <summary>
    /// Servicio para generar números secretos aleatorios de 4 dígitos únicos
    /// </summary>
    public interface ISecretNumberGenerator
    {
        /// <summary>
        /// Genera un número secreto de 4 dígitos sin repetir
        /// </summary>
        /// <returns>String con 4 dígitos únicos</returns>
        string Generate();
    }
}
