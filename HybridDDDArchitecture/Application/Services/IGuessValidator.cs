namespace Application.Services
{
    /// <summary>
    /// Servicio para validar intentos de adivinanza usando GuessCore
    /// </summary>
    public interface IGuessValidator
    {
        /// <summary>
        /// Valida un intento de adivinanza comparándolo con el número secreto
        /// </summary>
        /// <param name="secretNumber">Número secreto del juego</param>
        /// <param name="attemptedNumber">Número intentado por el jugador</param>
        /// <returns>Mensaje con las pistas (famas y picas)</returns>
        string Validate(string secretNumber, string attemptedNumber);
    }
}
