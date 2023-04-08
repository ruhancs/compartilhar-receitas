using System.Text.RegularExpressions;
using FluentValidation;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Exceptions;

namespace MeuLivroReceitas.Application.UseCases.User.Register;

//validar classe RequestRegisterUserJson
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        //ResourceMessageError gerado em shared Exceptions
        RuleFor(c => c.Name).NotEmpty().WithMessage(ResourceMessageError.EMPTY_USER);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ResourceMessageError.EMAIL_EMPTY);
        RuleFor(c => c.Password).NotEmpty().WithMessage(ResourceMessageError.PASSWORD_EMPTY);
        RuleFor(c => c.Phone).NotEmpty().WithMessage(ResourceMessageError.PHONE_EMPTY);
        
        When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ResourceMessageError.EMAIL_INVALID);
        });
        
        When(c => !string.IsNullOrWhiteSpace(c.Password), () =>
        {
            RuleFor(c => c.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessageError.PASSWORD_MIN_LENGTH);
        });
        
        When(c => !string.IsNullOrWhiteSpace(c.Phone), () =>
        {
            RuleFor(c => c.Phone).Custom((phone, context) =>
            {
                string phonePattern = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
                var isMatch = Regex.IsMatch(phone, phonePattern);
                if (!isMatch)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(phone), ResourceMessageError.PHONE_INVALID));
                }
            });
        });
    }
}
