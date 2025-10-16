using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .MinimumLength(4).When(x => !string.IsNullOrEmpty(x.Username))
                .WithMessage("Username must be at least 4 characters")
                .MaximumLength(30).WithMessage("Username must not exceed 30 characters")
                .Matches(@"^[a-zA-Z0-9_]+$").When(x => !string.IsNullOrEmpty(x.Username))
                .WithMessage("Username can only contain letters, numbers and underscores");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

            RuleFor(x => x.Password)
                .MinimumLength(8).When(x => !string.IsNullOrEmpty(x.Password))
                .WithMessage("Password must be at least 8 characters")
                .MaximumLength(50).WithMessage("Password must not exceed 50 characters");
        }
    }
}