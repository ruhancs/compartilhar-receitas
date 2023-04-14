using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.Code;
using MeuLivroReceitas.Domain.Repositories.Recipe;

namespace MeuLivroReceitas.Application.UseCases.Connection.QRCodeGenerator;

//para utilizala adicionar em Bootstraped em addUseCase
//para injecao de dependencia
public class QRCodeGeneratorUseCase : IQRCodeGeneratorUseCase
{
    private readonly ICodeWriteOnlyRepository  _repository;
    private readonly IAuthenticatedUser _user;
    private readonly IUnitOfWork _unitOfWork;

    public QRCodeGeneratorUseCase(
        ICodeWriteOnlyRepository repository,
        IAuthenticatedUser user,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _user = user;
        _unitOfWork = unitOfWork;
    }

    //utilizado em websockets AddConnection
    //para gerar qrcode e enviar
    public async Task<string> Execute()
    {
        //GetUser implementado em Application services authenticatedUser
        //pega o usuario utilizando o token do header
        var user = await _user.GetUser();

        //gerar o codigo
        var code = new Domain.Entities.Codes 
        {
            Code = Guid.NewGuid().ToString(),
            UserId = user.Id
        };

        await _repository.Register(code);

        await _unitOfWork.Commit();

        // para gerar o qrcode utilizar o plugin
        //que recebe string e devolve o qrcode
        return code.Code;
    }
}
