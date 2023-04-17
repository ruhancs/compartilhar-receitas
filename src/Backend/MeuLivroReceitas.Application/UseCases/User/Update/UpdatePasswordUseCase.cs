using FluentValidation.Results;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Application.Services.Cryptography;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.User;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.User.Update;

//adicionar injecao de dependencia no bootstraped de:
//IUpdatePasswordUseCase , UpdatePasswordUseCase
public class UpdatePasswordUseCase : IUpdatePasswordUseCase
{
    private readonly IUpdateOnlyRepository _repository;
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly EncryptPassword _encrypt;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePasswordUseCase(
        IUpdateOnlyRepository repository,
        IAuthenticatedUser authenticatedUser,
        EncryptPassword encrypt,
        IUnitOfWork unitOfWork
        )
    {
        _authenticatedUser = authenticatedUser;
        _repository = repository;
        _encrypt = encrypt;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(RequestUpdatePasswordJson req)
    {

        //para pegar o usuario que esta logado
        //e utilizar o id para buscar o usuario e atualizalo
        var userAuth = await _authenticatedUser.GetUser();

        var user = await _repository.GetById(userAuth.Id);

        Validate(req, user);

        user.Password = _encrypt.Cryptography(req.NewPassword);

        await _unitOfWork.Commit();

        _repository.Update(user);
    }

    private void Validate(RequestUpdatePasswordJson req, Usuario user)
    {
        var validator = new UpdatePasswordValidator();
        var result = validator.Validate(req);

        var currentPassword = _encrypt.Cryptography(req.Password);

        if(!user.Password.Equals(currentPassword))
        {
            result.Errors.Add(new ValidationFailure("currentpassword", ResourceMessageError.INVALID_CURRENT_PASSWORD));
        }

        if (!result.IsValid)
        {
            var messagesError = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ValidationErrors(messagesError);
        }
    }
}
