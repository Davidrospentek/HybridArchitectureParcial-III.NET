namespace Application.Services
{
    /// <summary>
    /// Implementación del validador de intentos usando ESCMB.GuessCore
    /// </summary>
    public class GuessValidator : IGuessValidator
    {
        /// <summary>
        /// Valida un intento de adivinanza comparándolo con el número secreto
        /// </summary>
        /// <param name="secretNumber">Número secreto del juego (4 dígitos)</param>
        /// <param name="attemptedNumber">Número intentado por el jugador (4 dígitos)</param>
        /// <returns>Mensaje con las pistas (ej: "Tu número tiene 2 fama y 1 pica")</returns>
        public string Validate(string secretNumber, string attemptedNumber)
        {
            // IMPORTANTE: Reemplazar esta implementación manual con el paquete ESCMB.GuessCore
            // cuando lo tengas instalado

            // Implementación manual temporal:
            int famas = 0;
            int picas = 0;

            for (int i = 0; i < 4; i++)
            {
                if (secretNumber[i] == attemptedNumber[i])
                {
                    famas++;
                }
                else if (secretNumber.Contains(attemptedNumber[i]))
                {
                    picas++;
                }
            }

            if (famas == 4)
            {
                return "¡Felicidades! Has adivinado el número.";
            }
            else
            {
                return $"Tu número tiene {famas} fama y {picas} pica";
            }

            // IMPLEMENTACIÓN CON GUESSCORE (descomenta cuando instales el paquete):
            // var guessCore = new ESCMB.GuessCore.GuessValidator();
            // return guessCore.Validate(secretNumber, attemptedNumber);
        }
    }
}
