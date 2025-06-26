using DevLife.Application.Features.Authentication.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Authentication.Validators;
 public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("Username cannot be longer than 20 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50);

        RuleFor(x => x.TechStack)
            .NotEmpty();

        RuleFor(x => x.ExperienceLevel)
            .NotEmpty();

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now.AddYears(-16)).WithMessage("User must be at least 16 years old.");
    }
}
