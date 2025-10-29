using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application;
using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Game.Commands.RegisterPlayer
{
    /// <summary>
    /// Comando para registrar un nuevo jugador en el sistema
    /// </summary>
    public class RegisterPlayerCommand : IRequestCommand<int>
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
        public int Age { get; set; }

        public RegisterPlayerCommand()
        {
        }
    }
}
