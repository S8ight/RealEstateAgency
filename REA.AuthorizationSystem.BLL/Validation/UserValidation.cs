using FluentValidation;
using REA.AuthorizationSystem.BLL.DTO.Request;

namespace REA.AuthorizationSystem.BLL.Validation;

public class UserValidation : AbstractValidator<RegistrationRequest>
{
    public UserValidation()
    {
        RuleFor(p => p.FirstName)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(2,30)
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

        RuleFor(p => p.LastName)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(2, 30)
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");
            
        RuleFor(p => p.Patronymic)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .Length(2, 30)
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(8, 30)
            .Matches("[A-Z]")
            .Matches("[a-z]")
            .Matches("[0-9]");

        RuleFor(p => p.UserName)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(4, 30);
            
        RuleFor(p => p.DateOfBirthd)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.");
    }

    private bool IsValidName(string name)
    {
        return name.All(Char.IsLetter);
    }
}