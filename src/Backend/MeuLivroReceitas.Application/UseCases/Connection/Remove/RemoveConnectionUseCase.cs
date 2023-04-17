using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Domain.Repositories.Connection;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Domain.Repositories;

namespace MeuLivroReceitas.Application.UseCases.Connection.Remove;

public class RemoveConnectionUseCase : IRemoveConnectionUseCase
{
    private readonly IAuthenticatedUser _user;
    private readonly IConnectionReadOnlyRepository _connectionReadOnlyRepository;
    private readonly IConnectionWriteOnlyRepository _connectionWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveConnectionUseCase(
        IAuthenticatedUser user,
        IConnectionReadOnlyRepository connectionReadOnlyRepository,
        IConnectionWriteOnlyRepository connectionWriteOnlyRepository,
        IUnitOfWork unitOfWork
        )
    {
        _user = user;
        _connectionReadOnlyRepository = connectionReadOnlyRepository;
        _connectionWriteOnlyRepository = connectionWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long userConnectedId)
    {
        var authenticatedUser = await _user.GetUser();

        var allConnections = await _connectionReadOnlyRepository.GetUserConnetions(authenticatedUser.Id);

        Validate(allConnections, userConnectedId);

        _connectionWriteOnlyRepository.Remove(authenticatedUser.Id, userConnectedId);

        await _unitOfWork.Commit();
    }

    public static void Validate(
        IList<Domain.Entities.Usuario> usersConnected,
        long userConnectedId
        )
    {

        if (!usersConnected.Any(c => c.Id == userConnectedId))
        {
            throw new ValidationErrors(new List<string> { ResourceMessageError.USER_NOT_CONNECTED });
        }
    }
}
