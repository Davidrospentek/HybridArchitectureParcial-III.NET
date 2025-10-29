using Core.Domain.Entities;
using Domain.Validators;

namespace Domain.Entities
{
    /// <summary>
    /// Entidad de dominio que representa un jugador del juego Picas y Famas
    /// </summary>
    public class Player : DomainEntity<int, PlayerValidator>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public DateTime RegisterDate { get; private set; }

        public Player()
        {
        }

        public Player(string firstName, string lastName, int age)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetAge(age);
            RegisterDate = DateTime.Now;
        }

        public Player(int id, string firstName, string lastName, int age, DateTime registerDate)
        {
            Id = id;
            SetFirstName(firstName);
            SetLastName(lastName);
            SetAge(age);
            RegisterDate = registerDate;
        }

        public void SetFirstName(string value)
        {
            FirstName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void SetLastName(string value)
        {
            LastName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public void SetAge(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Age must be greater than 0", nameof(value));
            Age = value;
        }
    }
}