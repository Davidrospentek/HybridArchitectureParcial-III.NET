using Core.Domain.Validators;
using Domain.Entities;
using FluentValidation;

namespace Domain.Validators
{
    public class AttemptValidator : EntityValidator<Attempt>
    {
        public AttemptValidator()
        {
            RuleFor(x => x.GameId)
                .GreaterThan(0).WithMessage("GameId must be greater than 0");

            RuleFor(x => x.AttemptedNumber)
                .NotEmpty().WithMessage("AttemptedNumber is required")
                .Length(4).WithMessage("AttemptedNumber must have exactly 4 digits")
                .Must(BeValidNumber).WithMessage("AttemptedNumber must contain 4 unique digits");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required");
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