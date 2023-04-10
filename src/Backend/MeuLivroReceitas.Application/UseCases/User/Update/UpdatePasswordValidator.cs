using System.Text.RegularExpressions;
using FluentValidation;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Exceptions;

namespace MeuLivroReceitas.Application.UseCases.User.Update;

public class UpdatePasswordValidator : AbstractValidator<RequestUpdatePasswordJson>
{
    public UpdatePasswordValidator()
    {
        //validacao de senha feita em ValidatePassword
        RuleFor(c => c.NewPassword).SetValidator(new ValidatePassword());

    }
}
