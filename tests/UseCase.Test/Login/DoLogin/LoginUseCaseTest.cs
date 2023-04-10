using FluentAssertions;
using MeuLivroReceitas.Application.UseCases.Login.DoLogin;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Domain.Entities;
using UtilsTests.Encrypt;
using UtilsTests.Entities;
using UtilsTests.Repositories;
using UtilsTests.Token;
using Xunit;

namespace UseCase.Test.Login.DoLogin;

public class LoginUseCaseTest
{
    [Fact]
    public async Task ValidateSuccess()
    {
        (var user, var password) = UserBuilder.Construct();

        var useCase = CreateUseCase(user);

        var response = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });
        //
        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    private LoginUseCase CreateUseCase(Usuario user)
    {
        
        var encryptPassword = EncryptPasswordBuilder.Intance();
        var tokenController = TokenControllerBuilder.Instance();
        var repositoryReadOnly = UserReadOnlyRepositoryBuilder.Instancia().LoginWithEmailAndPassword(user).Construct();

        return new LoginUseCase(repositoryReadOnly, encryptPassword, tokenController);
    }
}
