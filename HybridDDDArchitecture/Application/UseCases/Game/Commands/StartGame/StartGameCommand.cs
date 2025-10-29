using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Game.Commands.StartGame
{
    /// <summary>
    /// Comando para iniciar un nuevo juego de Picas y Famas
    /// </summary>
    public class StartGameCommand : IRequestCommand<StartGameResponse>
    {
        [Required(ErrorMessage = "PlayerId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PlayerId must be greater than 0")]
        public int PlayerId { get; set; }

        public StartGameCommand()
        {
        }
    }

    public class StartGameResponse
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}