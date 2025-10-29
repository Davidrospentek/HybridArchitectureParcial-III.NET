using Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Sql
{
    public class GameRepository : BaseRepository<Domain.Entities.Game>, IGameRepository
    {
        private readonly ILogger<GameRepository> _logger;

        public GameRepository(GameDbContext context, ILogger<GameRepository> logger)
            : base(context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Domain.Entities.Game> FindActiveGameByPlayerIdAsync(int playerId)
        {
            _logger.LogInformation("Searching for active game for PlayerId: {PlayerId}", playerId);

            try
            {
                var game = await Repository
                    .Where(g => g.PlayerId == playerId && !g.IsFinished)
                    .FirstOrDefaultAsync();

                if (game != null)
                {
                    _logger.LogInformation("Active game found: {GameId} for PlayerId: {PlayerId}",
                        game.Id, playerId);
                }
                else
                {
                    _logger.LogInformation("No active game found for PlayerId: {PlayerId}", playerId);
                }

                return game;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for active game for PlayerId: {PlayerId}", playerId);
                throw;
            }
        }

        public async Task<List<Domain.Entities.Game>> GetGamesByPlayerIdAsync(int playerId)
        {
            _logger.LogInformation("Getting all games for PlayerId: {PlayerId}", playerId);

            try
            {
                var games = await Repository
                    .Where(g => g.PlayerId == playerId)
                    .OrderByDescending(g => g.CreateAt)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} games for PlayerId: {PlayerId}",
                    games.Count, playerId);

                return games;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting games for PlayerId: {PlayerId}", playerId);
                throw;
            }
        }
    }
}