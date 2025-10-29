using Application.Constants;
using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Core.Application;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Game.Commands.GuessNumber
{
    /// <summary>
    /// Handler que procesa el comando de intento de adivinanza
    /// </summary>
    internal sealed class GuessNumberCommandHandler : IRequestCommandHandler<GuessNumberCommand, GuessNumberResponse>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IAttemptRepository _attemptRepository;
        private readonly IGuessValidator _guessValidator;
        private readonly ILogger<GuessNumberCommandHandler> _logger;

        public GuessNumberCommandHandler(
            IGameRepository gameRepository,
            IAttemptRepository attemptRepository,
            IGuessValidator guessValidator,
            ILogger<GuessNumberCommandHandler> logger)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _attemptRepository = attemptRepository ?? throw new ArgumentNullException(nameof(attemptRepository));
            _guessValidator = guessValidator ?? throw new ArgumentNullException(nameof(guessValidator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GuessNumberResponse> Handle(GuessNumberCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing guess for GameId: {GameId}, AttemptedNumber: {AttemptedNumber}",
                request.GameId, request.AttemptedNumber);

            try
            {
                // Validar formato del número
                if (request.AttemptedNumber.Length != 4 ||
                    !request.AttemptedNumber.All(char.IsDigit) ||
                    request.AttemptedNumber.Distinct().Count() != 4)
                {
                    _logger.LogWarning("Invalid number format: {AttemptedNumber}", request.AttemptedNumber);
                    throw new BussinessException("El número debe tener 4 dígitos únicos sin repetir");
                }

                // Obtener el juego
                var game = await _gameRepository.FindOneAsync(request.GameId);
                if (game == null)
                {
                    _logger.LogWarning("Game not found: {GameId}", request.GameId);
                    throw new EntityNotFoundException($"El juego con ID {request.GameId} no existe");
                }

                // Verificar si el juego ya finalizó
                if (game.IsFinished)
                {
                    _logger.LogWarning("Game already finished: {GameId}", request.GameId);
                    throw new BussinessException($"El juego {request.GameId} ya ha finalizado.");
                }

                // Validar el número usando GuessCore
                string message = _guessValidator.Validate(game.SecretNumber, request.AttemptedNumber);

                _logger.LogInformation("Guess validation result for GameId {GameId}: {Message}",
                    request.GameId, message);

                // Crear registro del intento
                var attempt = new Attempt(request.GameId, request.AttemptedNumber, message);

                if (!attempt.IsValid)
                {
                    _logger.LogError("Invalid attempt data for GameId: {GameId}", request.GameId);
                    throw new InvalidEntityDataException(attempt.GetErrors());
                }

                await _attemptRepository.AddAsync(attempt);

                // Si adivinó correctamente (4 famas), marcar el juego como finalizado
                if (message.Contains("¡Felicidades!") || message.Contains("4 fama"))
                {
                    game.MarkAsFinished();
                    _gameRepository.Update(game.Id, game);

                    _logger.LogInformation("Game {GameId} finished successfully", request.GameId);
                }

                return new GuessNumberResponse
                {
                    GameId = request.GameId,
                    AttemptedNumber = request.AttemptedNumber,
                    Message = message
                };
            }
            catch (Exception ex) when (ex is not EntityNotFoundException && ex is not BussinessException && ex is not InvalidEntityDataException)
            {
                _logger.LogError(ex, "Error processing guess for GameId: {GameId}", request.GameId);
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}
