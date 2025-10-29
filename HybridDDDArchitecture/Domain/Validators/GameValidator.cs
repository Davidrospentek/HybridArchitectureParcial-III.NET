using Core.Domain.Validators;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validators
{
    public class GameValidator : EntityValidator<Game>
    {
        public GameValidator()
        {
            RuleFor(x => x.PlayerId)
                .GreaterThan(0).WithMessage("PlayerId must be greater than 0");

            RuleFor(x => x.SecretNumber)
                .NotEmpty().WithMessage("SecretNumber is required")
                .Length(4).WithMessage("SecretNumber must have exactly 4 digits")
                .Must(BeValidNumber).WithMessage("SecretNumber must contain 4 unique digits");
        }

        private bool BeValidNumber(string number)
        {
            if (string.IsNullOrEmpty(number)) return false;
            if (number.Length != 4) return false;
            if (!number.All(char.IsDigit)) return false;
            return number.Distinct().Count() == 4;
        }
    }
}
