using HashidsNet;
using MeuLivroReceitas.Domain.Repositories.Code;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;

namespace MeuLivroReceitas.Application.UseCases.Connection.RefuseConnection;

//para utilizala adicionar em Bootstraped em addUseCase
//para injecao de dependencia
public class RefuseConnectionUseCase: IRefuseConnectionUseCase
{
    private readonly ICodeWriteOnlyRepository _repository;
    private readonly IAuthenticatedUser _user;
    private readonly IUnitOfWork _unitOfWork;
    private IHashids _hashId;
    public RefuseConnectionUseCase(
        ICodeWriteOnlyRepository repository,
        IAuthenticatedUser user,
        IUnitOfWork unitOfWork,
        IHashids hashId
        )
    {
        _repository = repository;
        _user = user;
        _unitOfWork = unitOfWork;
        _hashId = hashId;
    }

    public async Task<string> Execute()
    {
        var user = await _user.GetUser();

        await _repository.Delete(user.Id);

        await _unitOfWork.Commit();

        return _hashId.EncodeLong(user.Id);
    }
}
