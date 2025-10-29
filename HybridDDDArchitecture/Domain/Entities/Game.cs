using Core.Domain.Entities;
using Domain.Validators;

namespace Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un juego de Picas y Famas
    /// </summary>
    public class Game : DomainEntity<int, GameValidator>
    {
        public int PlayerId { get; private set; }
        public string SecretNumber { get; private set; }
        public DateTime CreateAt { get; private set; }
        public bool IsFinished { get; private set; }

        public Game()
        {
        }

        public Game(int playerId, string secretNumber)
        {
            PlayerId = playerId;
            SetSecretNumber(secretNumber);
            CreateAt = DateTime.Now;
            IsFinished = false;
        }

        public Game(int id, int playerId, string secretNumber, DateTime createAt, bool isFinished)
        {
            Id = id;
            PlayerId = playerId;
            SetSecretNumber(secretNumber);
            CreateAt = createAt;
            IsFinished = isFinished;
        }

        public void SetSecretNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (value.Length != 4)
                throw new ArgumentException("Secret number must have 4 digits", nameof(value));

            if (!value.All(char.IsDigit))
                throw new ArgumentException("Secret number must contain only digits", nameof(value));

            if (value.Distinct().Count() != 4)
                throw new ArgumentException("Secret number must not have repeated digits", nameof(value));

            SecretNumber = value;
        }

        public void MarkAsFinished()
        {
            IsFinished = true;
        }
    }
}