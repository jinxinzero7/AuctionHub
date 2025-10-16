using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class UpdateLotRequestValidator : AbstractValidator<LotUpdateRequest>
    {
        public UpdateLotRequestValidator()
        {
            RuleFor(x => x.Title)
                .MinimumLength(4).When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("Title must be at least 4 characters")
                .MaximumLength(30).When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("Title must not exceed 30 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_,\.]+$").When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("Title can only contain letters, numbers, spaces, and basic punctuation");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.StartingPrice)
                .GreaterThan(0).When(x => x.StartingPrice > 0)
                .WithMessage("Starting price must be greater than 0")
                .LessThan(1000000).When(x => x.StartingPrice > 0)
                .WithMessage("Starting price must be less than 1,000,000");

            RuleFor(x => x.BidIncrement)
                .GreaterThan(0).When(x => x.BidIncrement > 0)
                .WithMessage("Bid increment must be greater than 0")
                .LessThan(100000).When(x => x.BidIncrement > 0)
                .WithMessage("Bid increment must be less than 100,000");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.UtcNow).When(x => x.EndDate != default)
                .WithMessage("End date must be in the future")
                .LessThan(DateTime.UtcNow.AddYears(1)).When(x => x.EndDate != default)
                .WithMessage("Auction cannot be longer than 1 year")
                .Must(BeReasonableEndDate).When(x => x.EndDate != default)
                .WithMessage("Auction should be at least 1 hour long");

            RuleFor(x => x.ImageUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.ImageUrl))
                .WithMessage("ImageUrl must be a valid URL")
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.ImageUrl))
                .WithMessage("ImageUrl must not exceed 500 characters");
        }

        private bool BeValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return true;
            return Uri.TryCreate(url, UriKind.Absolute, out _) && 
                   (url.StartsWith("http://") || url.StartsWith("https://"));
        }

        private bool BeReasonableEndDate(DateTime endDate)
        {
            return endDate > DateTime.UtcNow.AddHours(1);
        }
    }
}