using Application.Constants;
using Application.Exceptions;
using Application.Repositories;
using Core.Application;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Game.Commands.RegisterPlayer
{
    /// <summary>
    /// Handler que procesa el comando de registro de jugador
    /// </summary>
    internal sealed class RegisterPlayerCommandHandler : IRequestCommandHandler<RegisterPlayerCommand, int>
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<RegisterPlayerCommandHandler> _logger;

        public RegisterPlayerCommandHandler(
            IPlayerRepository playerRepository,
            ILogger<RegisterPlayerCommandHandler> logger)
        {
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(RegisterPlayerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting player registration process for {FirstName} {LastName}",
                request.FirstName, request.LastName);

            try
            {
                // Verificar si el jugador ya existe
                var existingPlayer = await _playerRepository.FindByNameAsync(request.FirstName, request.LastName);

                if (existingPlayer != null)
                {
                    _logger.LogWarning("Player already exists: {FirstName} {LastName}",
                        request.FirstName, request.LastName);
                    throw new EntityDoesExistException("El jugador ya se encuentra registrado");
                }

                // Crear nueva entidad de jugador
                var player = new Player(request.FirstName, request.LastName, request.Age);

                // Validar entidad de dominio
                if (!player.IsValid)
                {
                    _logger.LogError("Invalid player data for {FirstName} {LastName}",
                        request.FirstName, request.LastName);
                    throw new InvalidEntityDataException(player.GetErrors());
                }

                // Persistir en base de datos
                var createdId = await _playerRepository.AddAsync(player);

                _logger.LogInformation("Player registered successfully with ID: {PlayerId}", createdId);

                return Convert.ToInt32(createdId);
            }
            catch (Exception ex) when (ex is not EntityDoesExistException && ex is not InvalidEntityDataException)
            {
                _logger.LogError(ex, "Error during player registration for {FirstName} {LastName}",
                    request.FirstName, request.LastName);
                throw new BussinessException(ApplicationConstants.PROCESS_EXECUTION_EXCEPTION, ex);
            }
        }
    }
}