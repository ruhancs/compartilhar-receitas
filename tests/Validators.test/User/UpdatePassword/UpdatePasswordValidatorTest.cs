using FluentAssertions;
using FluentValidation;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Application.UseCases.User.Update;
using MeuLivroReceitas.Exceptions;
using UtilsTests.Requests;
using Xunit;

namespace Validators.test.User.UpdatePassword;

public class UpdatePasswordValidatorTest
{
    [Fact]//informa que é um test
    public void ValidateSuccess()
    {
        var validator = new UpdatePasswordValidator();

        var req = RequestUpdatePasswordBuilder.Construct();

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeTrue();
    }

    [Fact]//informa que é um test
    public void ValidateErroEmptyPassword()
    {
        var validator = new UpdatePasswordValidator();

        var req = RequestUpdatePasswordBuilder.Construct();
        req.NewPassword = string.Empty;

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.PASSWORD_EMPTY));
    }
}
