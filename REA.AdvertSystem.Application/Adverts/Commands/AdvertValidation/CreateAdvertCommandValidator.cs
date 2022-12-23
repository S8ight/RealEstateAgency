using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REA.AdvertSystem.Application.Adverts.Commands.AdvertValidation
{
    public class CreateAdvertCommandValidator : AbstractValidator<CreateAdvertCommand>
    {
        public CreateAdvertCommandValidator()
        {
            RuleFor(a => a.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(150).WithMessage("Title must not exceed 150 characters.");

            RuleFor(a => a.Description)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1500).WithMessage("Title must not exceed 1500 characters.");
            
            RuleFor(a => a.Adress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Adress is required.");

            RuleFor(a => a.Square)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Square is required.")
                .GreaterThan(0).WithMessage("Square can not be zero");

            RuleFor(a => a.Price)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price can not be zero");
        }
    }
}
