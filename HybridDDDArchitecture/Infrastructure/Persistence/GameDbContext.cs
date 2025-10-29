using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Contexto de base de datos para el juego Picas y Famas
    /// </summary>
    public class GameDbContext : DbContext
    {
        private readonly ILogger<GameDbContext> _logger;

        public DbSet<Player> Players { get; set; }
        public DbSet<Domain.Entities.Game> Games { get; set; }
        public DbSet<Attempt> Attempts { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options, ILogger<GameDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Player
            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Players");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.RegisterDate).IsRequired();

                // Índice para búsqueda por nombre
                entity.HasIndex(e => new { e.FirstName, e.LastName });

                _logger.LogInformation("Player entity configured");
            });

            // Configuración de la entidad Game
            modelBuilder.Entity<Domain.Entities.Game>(entity =>
            {
                entity.ToTable("Games");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.PlayerId).IsRequired();
                entity.Property(e => e.SecretNumber).IsRequired().HasMaxLength(4);
                entity.Property(e => e.CreateAt).IsRequired();
                entity.Property(e => e.IsFinished).IsRequired().HasDefaultValue(false);

                // Índice para búsqueda de juegos activos por jugador
                entity.HasIndex(e => new { e.PlayerId, e.IsFinished });

                _logger.LogInformation("Game entity configured");
            });

            // Configuración de la entidad Attempt
            modelBuilder.Entity<Attempt>(entity =>
            {
                entity.ToTable("Attempts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.GameId).IsRequired();
                entity.Property(e => e.AttemptedNumber).IsRequired().HasMaxLength(4);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(500);
                entity.Property(e => e.AttemptDate).IsRequired();

                // Índice para búsqueda de intentos por juego
                entity.HasIndex(e => e.GameId);
                entity.HasIndex(e => e.AttemptDate);

                _logger.LogInformation("Attempt entity configured");
            });
        }

        public override int SaveChanges()
        {
            _logger.LogInformation("Saving changes to database");
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Saving changes to database asynchronously");
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
