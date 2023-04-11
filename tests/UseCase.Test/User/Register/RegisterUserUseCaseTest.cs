using FluentAssertions;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using UtilsTests.Encrypt;
using UtilsTests.Mapper;
using UtilsTests.Repositories;
using UtilsTests.Requests;
using UtilsTests.Token;
using Xunit;

namespace UseCase.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task ValidatSuccess()
    {
        var request = RequestRegisterUserBuilder.Construct();

        var useCase = CreateUseCase();

        //resposta é ResponseRegisterUserJson
        var response = await useCase.Execute(request);

        //response deve ser um token
        response.Should().NotBeNull();
        response.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task ValidatEmailAlreadyExist()
    {
        var request = RequestRegisterUserBuilder.Construct();

        var useCase = CreateUseCase(request.Email);

        Func<Task> acao = async () => { await useCase.Execute(request); };

        //acao é a resposta
        //resposta é ResponseRegisterUserJson
        //response deve ser um token
        await acao.Should().ThrowAsync<ValidationErrors>()
            .Where(
                err =>
                    err.MessagesErro.Count == 1 && err.MessagesErro.Contains(ResourceMessageError.EMAIL_ALREADY_EXIST)
                );
    }

    //criar instancia de RegisterUserUseCase
    private static RegisterUserUseCase CreateUseCase(string email = "")
    {
        var mapper = MapperBuilder.Instance();
        var repository = UserWriteOnlyRepositoryBuilder.Instancia().Construct();
        var unitOfWork = UnitOfWorkBuilder.Instance().Construct();
        var encryptPassword = EncryptPasswordBuilder.Intance();
        var tokenController = TokenControllerBuilder.Instance();
        var repositoryReadOnly = UserReadOnlyRepositoryBuilder.Instancia().ExistUserEmail(email).Construct();

        return new RegisterUserUseCase(
            repository,
            mapper,
            unitOfWork,
            encryptPassword,
            tokenController,
            repositoryReadOnly
            );
    }
}