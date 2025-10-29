namespace Application.Constants
{
    /// <summary>
    /// Constantes utilizadas en la capa de aplicación
    /// </summary>
    public static class ApplicationConstants
    {
        // Mensajes de error generales
        public const string PROCESS_EXECUTION_EXCEPTION = "Ocurrió un error durante la ejecución del proceso";

        // Mensajes relacionados con jugadores
        public const string PLAYER_ALREADY_EXISTS = "El jugador ya se encuentra registrado";
        public const string PLAYER_NOT_FOUND = "El jugador no está registrado";

        // Mensajes relacionados con juegos
        public const string GAME_NOT_FOUND = "El juego no existe";
        public const string GAME_ALREADY_FINISHED = "El juego ya ha finalizado";
        public const string ACTIVE_GAME_EXISTS = "El jugador ya tiene un juego activo";

        // Mensajes de validación
        public const string INVALID_NUMBER_FORMAT = "El número debe tener 4 dígitos únicos sin repetir";
    }
}
