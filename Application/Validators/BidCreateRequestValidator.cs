using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class BidCreateRequestValidator : AbstractValidator<BidCreateRequest>
    {
        public BidCreateRequestValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Bid amount is required")
                .GreaterThan(0).WithMessage("Bid amount must be greater than 0")
                .LessThan(1000000).WithMessage("Bid amount must be less than 1,000,000")
                .PrecisionScale(18, 2, false).WithMessage("Bid amount cannot have more than 2 decimal places");
        }
    }
}