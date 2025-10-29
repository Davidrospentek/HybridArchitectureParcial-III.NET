using Core.Domain.Entities;
using Domain.Validators;

namespace Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un intento de adivinar el número secreto
    /// </summary>
    public class Attempt : DomainEntity<int, AttemptValidator>
    {
        public int GameId { get; private set; }
        public string AttemptedNumber { get; private set; }
        public string Message { get; private set; }
        public DateTime AttemptDate { get; private set; }

        public Attempt()
        {
        }

        public Attempt(int gameId, string attemptedNumber, string message)
        {
            GameId = gameId;
            SetAttemptedNumber(attemptedNumber);
            SetMessage(message);
            AttemptDate = DateTime.Now;
        }

        public Attempt(int id, int gameId, string attemptedNumber, string message, DateTime attemptDate)
        {
            Id = id;
            GameId = gameId;
            SetAttemptedNumber(attemptedNumber);
            SetMessage(message);
            AttemptDate = attemptDate;
        }

        public void SetAttemptedNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (value.Length != 4)
                throw new ArgumentException("Attempted number must have 4 digits", nameof(value));

            if (!value.All(char.IsDigit))
                throw new ArgumentException("Attempted number must contain only digits", nameof(value));

            if (value.Distinct().Count() != 4)
                throw new ArgumentException("Attempted number must not have repeated digits", nameof(value));

            AttemptedNumber = value;
        }

        public void SetMessage(string value)
        {
            Message = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}