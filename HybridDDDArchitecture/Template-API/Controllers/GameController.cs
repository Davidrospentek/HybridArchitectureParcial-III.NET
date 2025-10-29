using Application.UseCases.Game.Commands.GuessNumber;
using Application.UseCases.Game.Commands.RegisterPlayer;
using Application.UseCases.Game.Commands.StartGame;
using Controllers;
using Core.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    /// <summary>
    /// Controlador para el juego Picas y Famas
    /// </summary>
    [ApiController]
    [Route("api/game/v1")]
    public class GameController : BaseController
    {
        private readonly ICommandQueryBus _commandQueryBus;
        private readonly ILogger<GameController> _logger;

        public GameController(ICommandQueryBus commandQueryBus, ILogger<GameController> logger)
        {
            _commandQueryBus = commandQueryBus ?? throw new ArgumentNullException(nameof(commandQueryBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Registra un nuevo jugador en el sistema
        /// </summary>
        /// <param name="command">Datos del jugador a registrar</param>
        /// <returns>ID del jugador registrado</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(HttpMessageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterPlayerCommand command)
        {
            _logger.LogInformation("POST /api/game/v1/register - Registering player: {FirstName} {LastName}",
                command.FirstName, command.LastName);

            if (command is null)
            {
                _logger.LogWarning("Register request with null command");
                return BadRequest(new { message = "Los datos del jugador son requeridos" });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Register request with invalid model state");
                return BadRequest(ModelState);
            }

            var playerId = await _commandQueryBus.Send(command);

            _logger.LogInformation("Player registered successfully with ID: {PlayerId}", playerId);

            return Ok(new { playerId });
        }

        /// <summary>
        /// Inicia un nuevo juego para un jugador
        /// </summary>
        /// <param name="command">ID del jugador</param>
        /// <returns>Datos del juego creado</returns>
        [HttpPost("start")]
        [ProducesResponseType(typeof(HttpMessageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StartGame([FromBody] StartGameCommand command)
        {
            _logger.LogInformation("POST /api/game/v1/start - Starting game for PlayerId: {PlayerId}",
                command.PlayerId);

            if (command is null)
            {
                _logger.LogWarning("StartGame request with null command");
                return BadRequest(new { message = "El ID del jugador es requerido" });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("StartGame request with invalid model state");
                return BadRequest(ModelState);
            }

            var response = await _commandQueryBus.Send(command);

            _logger.LogInformation("Game started successfully: GameId {GameId} for PlayerId {PlayerId}",
                response.GameId, response.PlayerId);

            return Ok(response);
        }

        /// <summary>
        /// Procesa un intento de adivinar el número secreto
        /// </summary>
        /// <param name="command">ID del juego y número intentado</param>
        /// <returns>Resultado del intento con pistas</returns>
        [HttpPost("guess")]
        [ProducesResponseType(typeof(HttpMessageResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GuessNumber([FromBody] GuessNumberCommand command)
        {
            _logger.LogInformation("POST /api/game/v1/guess - Processing guess for GameId: {GameId}, Number: {AttemptedNumber}",
                command.GameId, command.AttemptedNumber);

            if (command is null)
            {
                _logger.LogWarning("GuessNumber request with null command");
                return BadRequest(new { message = "Los datos del intento son requeridos" });
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("GuessNumber request with invalid model state");
                return BadRequest(ModelState);
            }

            var response = await _commandQueryBus.Send(command);

            _logger.LogInformation("Guess processed for GameId: {GameId}, Result: {Message}",
                command.GameId, response.Message);

            return Ok(response);
        }
    }
}