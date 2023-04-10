using MeuLivroReceitas.Domain.Repositories.User;
using Moq;

namespace UtilsTests.Repositories;

//mock para simular UserWriteOnlyRepository que contem a funcao Add para addicionar user ao db
//UserWriteOnlyRepositoryBuilder precisa ser passado como parametro em RegisterUseCaseTest
public class UserWriteOnlyRepositoryBuilder
{
    private static UserWriteOnlyRepositoryBuilder _instance;
    private readonly Mock<IUserWriteOnlyRepository> _repository;

    private UserWriteOnlyRepositoryBuilder()
    {
        if(_repository == null)
        {
            _repository = new Mock<IUserWriteOnlyRepository>();
        }
    }

    public static UserWriteOnlyRepositoryBuilder Instancia()
    {
        _instance = new UserWriteOnlyRepositoryBuilder();
        return _instance;
    }

    public IUserWriteOnlyRepository Construct()
    {
        return _repository.Object;
    }
}
