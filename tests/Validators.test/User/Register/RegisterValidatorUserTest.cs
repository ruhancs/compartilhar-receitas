using FluentAssertions;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Exceptions;
using UtilsTests.Requests;
using Xunit;

namespace Validators.test.User.Register;

public class RegisterValidatorUserTest
{
    //cada funcao fara um test

    [Fact]//informa que é um test
    public void ValidateSuccess()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeTrue();
    }    
    
    [Fact]//informa que é um test
    public void ValidateErroEmptyName()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();
        req.Name = string.Empty;

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.EMPTY_USER));
    }

    [Fact]//informa que é um test
    public void ValidateErroEmptyEmail()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();
        req.Email = string.Empty;

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.EMAIL_EMPTY));
    }

    [Fact]//informa que é um test
    public void ValidateErroEmptyPassword()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();
        req.Password = string.Empty;

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.PASSWORD_EMPTY));
    }

    [Fact]//informa que é um test
    public void ValidateErroEmptyPhone()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();
        req.Phone = string.Empty;

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.PHONE_EMPTY));
    }

    [Fact]//informa que é um test
    public void ValidateErroShortPassword()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();
        req.Password = "1234";

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.PASSWORD_MIN_LENGTH));
    }

    [Fact]//informa que é um test
    public void ValidateErroInvalidEmail()
    {
        var validator = new RegisterUserValidator();

        var req = RequestRegisterUserBuilder.Construct();
        req.Email = "email";

        //valida requisicao e retorna o resultado
        var result = validator.Validate(req);

        //Should vem do pacote fluentAssertions
        result.IsValid.Should().BeFalse();
        result
            .Errors.Should()
            .ContainSingle()
            .And.Contain(err => err.ErrorMessage.Equals(ResourceMessageError.EMAIL_INVALID));
    }
}
