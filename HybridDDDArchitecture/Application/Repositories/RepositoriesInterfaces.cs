using Core.Application.Repositories;
using Domain.Entities;

namespace Application.Repositories
{
    /// <summary>
    /// Interfaz del repositorio de jugadores
    /// </summary>
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<Player> FindByNameAsync(string firstName, string lastName);
    }

    /// <summary>
    /// Interfaz del repositorio de juegos
    /// </summary>
    public interface IGameRepository : IRepository<Domain.Entities.Game>
    {
        Task<Domain.Entities.Game> FindActiveGameByPlayerIdAsync(int playerId);
        Task<List<Domain.Entities.Game>> GetGamesByPlayerIdAsync(int playerId);
    }

    /// <summary>
    /// Interfaz del repositorio de intentos
    /// </summary>
    public interface IAttemptRepository : IRepository<Attempt>
    {
        Task<List<Attempt>> GetAttemptsByGameIdAsync(int gameId);
        Task<int> CountAttemptsByGameIdAsync(int gameId);
    }
}
