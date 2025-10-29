using Application.Repositories;
using Core.Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio de intentos
    /// </summary>
    public class IAttemptRepository : BaseRepository<Attempt>, Application.Repositories.IAttemptRepository
    {
        private readonly ILogger<IAttemptRepository> _logger;

        public IAttemptRepository(GameDbContext context, ILogger<IAttemptRepository> logger)
            : base(context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Attempt>> GetAttemptsByGameIdAsync(int gameId)
        {
            _logger.LogInformation("Getting attempts for GameId: {GameId}", gameId);

            try
            {
                var attempts = await Repository
                    .Where(a => a.GameId == gameId)
                    .OrderBy(a => a.AttemptDate)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} attempts for GameId: {GameId}",
                    attempts.Count, gameId);

                return attempts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting attempts for GameId: {GameId}", gameId);
                throw;
            }
        }

        public async Task<int> CountAttemptsByGameIdAsync(int gameId)
        {
            _logger.LogInformation("Counting attempts for GameId: {GameId}", gameId);

            try
            {
                var count = await Repository
                    .Where(a => a.GameId == gameId)
                    .CountAsync();

                _logger.LogInformation("GameId {GameId} has {Count} attempts", gameId, count);

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting attempts for GameId: {GameId}", gameId);
                throw;
            }
        }
    }
}