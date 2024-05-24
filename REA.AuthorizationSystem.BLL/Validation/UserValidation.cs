using FluentValidation;
using REA.AuthorizationSystem.BLL.DTO.Request;

namespace REA.AuthorizationSystem.BLL.Validation;

public class UserValidation : AbstractValidator<RegistrationRequest>
{
    public UserValidation()
    {
        RuleFor(p => p.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(2,30)
            .WithMessage("{PropertyName} should be between {MinLength} and {MaxLength} characters.")
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

        RuleFor(p => p.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(2, 30)
            .WithMessage("{PropertyName} should be between {MinLength} and {MaxLength} characters.")
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");
            
        RuleFor(p => p.Patronymic)
            .Cascade(CascadeMode.Stop)
            .Length(2, 30)
            .WithMessage("{PropertyName} should be between {MinLength} and {MaxLength} characters.")
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.")
            .When(p => !string.IsNullOrWhiteSpace(p.Patronymic));


        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(8, 30)
            .WithMessage("{PropertyName} should be between {MinLength} and {MaxLength} characters.")
            .Matches("[A-Z]")
            .Matches("[a-z]")
            .Matches("[0-9]")
            .WithMessage("Password must consist of at least one uppercase, lowercase letter and a number.");

        RuleFor(p => p.PasswordConfirmation)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should not be empty.")
            .Equal(p => p.Password)
            .WithMessage("Password confirmation must be the same as the password.");

        RuleFor(p => p.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should be not empty.")
            .Length(4, 30)
            .WithMessage("{PropertyName} should be between {MinLength} and {MaxLength} characters.");

        // RuleFor(p => p.DateOfBirth)
        //     .Cascade(CascadeMode.Stop)
        //     .NotEmpty()
        //     .WithMessage("{PropertyName} should be not empty.");
        
        RuleFor(p => p.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should not be empty.")
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("{PropertyName} should be a valid phone number in international format, starting with '+' and followed by up to 15 digits.");
        
        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} should not be empty.")
            .EmailAddress()
            .WithMessage("{PropertyName} should be a valid email address.");
    }

    private bool IsValidName(string name)
    {
        return name.All(Char.IsLetter);
    }
}