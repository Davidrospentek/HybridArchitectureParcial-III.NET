namespace Application.Services
{
    /// <summary>
    /// Implementación del generador de números secretos
    /// </summary>
    public class SecretNumberGenerator : ISecretNumberGenerator
    {
        private readonly Random _random = new();

        /// <summary>
        /// Genera un número secreto de 4 dígitos sin repetir
        /// </summary>
        /// <returns>String con 4 dígitos únicos (ej: "5604")</returns>
        public string Generate()
        {
            // Generar 4 dígitos únicos aleatorios del 0 al 9
            var digits = Enumerable.Range(0, 10)
                .OrderBy(_ => _random.Next())
                .Take(4)
                .ToList();

            return string.Join("", digits);
        }
    }
}
