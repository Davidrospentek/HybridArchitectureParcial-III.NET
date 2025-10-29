using Application.Constants;
using Application.Exceptions;
using Application.Repositories;
using Application.Services;
using Core.Application;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Game.Commands.StartGame
{
    /// <summary>
    /// Handler que procesa el comando de inicio de juego
    /// </summary>
    internal sealed class StartGameCommandHandler : IRequestCommandHandler<StartGameCommand, StartGameResponse>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ISecretNumberGenerator _secretNumberGenerator;
        private readonly ILogger<StartGameCommandHandler> _logger;

        public StartGameCommandHandler(
            IGameRepository gameRepository,
            IPlayerRepository playerRepository,
            ISecretNumberGenerator secretNumberGenerator,
            ILogger<StartGameCommandHandler> logger)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _secretNumberGenerator = secretNumberGenerator ?? throw new ArgumentNullException(nameof(secretNumberGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<StartGameResponse> Handle(StartGameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting game for PlayerId: {PlayerId}", request.PlayerId);

            try
            {
                // Validar que el jugador exista
                var player = await _playerRepository.FindOneAsync(request.PlayerId);
                if (player == null)
                {
                    _logger.LogWarning("Player not found: {PlayerId}", request.PlayerId);
                    throw new EntityNotFoundException($"El jugador con ID {request.PlayerId} no está registrado");
                }

                // Verificar si el jugador tiene un juego activo
                var activeGame = await _gameRepository.FindActiveGameByPlayerIdAsync(request.PlayerId);
                if (activeGame != null)
                {
                    _logger.LogWarning("Player {PlayerId} already has an active game: {GameId}",
                        request.PlayerId, activeGame.Id);
                    throw new BussinessException($"El jugador ya tiene un juego activo (ID: {activeGame.Id}). Debe finalizar el juego antes de comenzar uno nuevo.");
                }

                // Generar número secreto
                string secretNumber = _secretNumberGenerator.Generate();
                _logger.LogInformation("Secret number generated for PlayerId: {PlayerId}", request.PlayerId);

                // Crear nueva entidad de juego
                var game = new Domain.Entities.Game(request.PlayerId, secretNumber);

                // Validar entidad de dominio
                if (!game.IsValid)
                {
                    _logger.LogError("Invalid game data for PlayerId: {PlayerId}", request.PlayerId);
                    throw new InvalidEntityDataException(game.GetErrors());
                }

                // Persistir en base de datos
                var createdId = await _gameRepository.AddAsync(game);
                game = await _gameRepository.FindOneAsync(Convert.ToInt32(createdId));

                _logger.LogInformation("Game created successfully with ID: {GameId} for PlayerId: {PlayerId}",
                    createdId, request.PlayerId);

                return new StartGameResponse
                {
                    GameId = game.Id,
                    PlayerId = game.PlayerId,
                    CreateAt = game.CreateAt
                };
            }
            catch (Exception ex) when (ex is not EntityNotFoundException && ex is not BussinessException && ex is not InvalidEntityDataException)
            {
                _logger.LogError(ex, "Error starting game for PlayerId: {PlayerId}", request.PlayerId);
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}