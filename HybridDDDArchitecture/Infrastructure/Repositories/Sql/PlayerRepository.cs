using Application.Repositories;
using Core.Infraestructure.Repositories.Sql;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Sql
{
    public class PlayerRepository : BaseRepository<Player>, IPlayerRepository
    {
        private readonly ILogger<PlayerRepository> _logger;

        public PlayerRepository(GameDbContext context, ILogger<PlayerRepository> logger)
            : base(context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Player> FindByNameAsync(string firstName, string lastName)
        {
            _logger.LogInformation("Searching for player: {FirstName} {LastName}", firstName, lastName);

            try
            {
                var player = await Repository
                    .Where(p => p.FirstName == firstName && p.LastName == lastName)
                    .FirstOrDefaultAsync();

                if (player != null)
                {
                    _logger.LogInformation("Player found: {PlayerId}", player.Id);
                }
                else
                {
                    _logger.LogInformation("Player not found: {FirstName} {LastName}", firstName, lastName);
                }

                return player;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for player: {FirstName} {LastName}", firstName, lastName);
                throw;
            }
        }
    }
}
