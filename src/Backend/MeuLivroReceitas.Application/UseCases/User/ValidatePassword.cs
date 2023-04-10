using FluentValidation;
using MeuLivroReceitas.Exceptions;

namespace MeuLivroReceitas.Application.UseCases.User;

//validar a senha para utilizar em
//RegisterUserUseCase e UpdatePasswordUseCase
public class ValidatePassword : AbstractValidator<string>
{
    public ValidatePassword()
    {
        //c é a string da senha
        RuleFor(password => password).NotEmpty().WithMessage(ResourceMessageError.PASSWORD_EMPTY);

        When(password => !string.IsNullOrWhiteSpace(password), () =>
        {
            RuleFor(password => password.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceMessageError.PASSWORD_MIN_LENGTH);
        });
    }
}
