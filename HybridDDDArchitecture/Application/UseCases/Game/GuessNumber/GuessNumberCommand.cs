using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Game.Commands.GuessNumber
{
    /// <summary>
    /// Comando para intentar adivinar el número secreto
    /// </summary>
    public class GuessNumberCommand : IRequestCommand<GuessNumberResponse>
    {
        [Required(ErrorMessage = "GameId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "GameId must be greater than 0")]
        public int GameId { get; set; }

        [Required(ErrorMessage = "AttemptedNumber is required")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "AttemptedNumber must be a 4-digit number")]
        public string AttemptedNumber { get; set; }

        public GuessNumberCommand()
        {
        }
    }

    public class GuessNumberResponse
    {
        public int GameId { get; set; }
        public string AttemptedNumber { get; set; }
        public string Message { get; set; }
    }
}
