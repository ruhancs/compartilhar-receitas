using HashidsNet;
using MeuLivroReceitas.Domain.Repositories.Code;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.Connection;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;

namespace MeuLivroReceitas.Application.UseCases.Connection.acceptConnection;

public class AcceptConnectionUseCase : IAcceptConnectionUseCase
{
    private readonly ICodeWriteOnlyRepository _repositoryCodeWrite;
    private readonly IAuthenticatedUser _user;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashids _hashId;
    private readonly IConnectionWriteOnlyRepository _repositoryConnectionWrite;
    public AcceptConnectionUseCase(
        ICodeWriteOnlyRepository repositoryCodeWrite,
        IAuthenticatedUser user,
        IUnitOfWork unitOfWork,
        IHashids hashId,
        IConnectionWriteOnlyRepository repositoryConnectionWrite
        )
    {
        _repositoryCodeWrite = repositoryCodeWrite;
        _user = user;
        _unitOfWork = unitOfWork;
        _hashId = hashId;
        _repositoryConnectionWrite = repositoryConnectionWrite;
    }

    public async Task<string> Execute(string userToConnect)
    {
        //usuario que gerou o qrcode
        var user = await _user.GetUser();

        //deletar o codigo gerado pelo usuario
        await _repositoryCodeWrite.Delete(user.Id);

        //id do usuario que leu qrCode
        var userReaderId = _hashId.DecodeLong(userToConnect).First();

        //criar conexao
        var connetiontoOwnerQRCode = new Domain.Entities.Conexao
        {
            UserId = user.Id,
            UserConnectedId = userReaderId
        };
        var connetiontoReaderQRCode = new Domain.Entities.Conexao
        {
            UserId = userReaderId,
            UserConnectedId = user.Id
        };
        //registrar a conexao dos usuarios
        //para um usuario poder ver as receitas do outro
        await _repositoryConnectionWrite.Register(connetiontoOwnerQRCode);
        await _repositoryConnectionWrite.Register(connetiontoReaderQRCode);

        await _unitOfWork.Commit();

        return _hashId.EncodeLong(user.Id);
    }
}
